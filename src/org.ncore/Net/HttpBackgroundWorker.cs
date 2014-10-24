using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using org.ncore.Diagnostics;

namespace org.ncore.Net
{
    public class HttpBackgroundWorker : BackgroundWorker
    {
        // NOTE: This is a little weird to not use an enum, but all the MS stuff
        //  uses the string type for Method so I'm just reflecting that.  I could
        //  imagine that they're trying to allow for custom/new http verbs, but 
        //  it also looks like they're checking its "validity" on the HttpWebRequest
        //  object.  Ugh.  JF
        public static class Method
        {
            public static readonly string GET = "GET";
            public static readonly string POST = "POST";
            public static readonly string PUT = "PUT";
            public static readonly string HEAD = "HEAD";
            public static readonly string DELETE = "DELETE";
        }

        // NOTE: No property setters so we don't have to mess with locks and worry about thread saftey (hopefully).  JF
        public HttpRequestBroker RequestBroker;
        public HttpResponseBroker ResponseBroker;

        // TODO: I don't really like this exact pattern, but there should be some way to pass some state info
        //  from the .RunWorkerAsync() caller to the .RunWorkerCompleted() event.  You'd think that the 
        //  .RunWorkerAsync( object argument ) would be available to .RunWorkerCompleted() but it's not.  Only
        //  visible to .DoWork().  Werird.  JF
        // TODO: Need to figure out how to implement correctly to support cancellation.  JF
        public object Source;

        public HttpBackgroundWorker( HttpRequestBroker requestBroker )
        {
            this.WorkerSupportsCancellation = true;

            RequestBroker = requestBroker;
        }

        // HACK: This is really just here for testing purposes.  Allows the worker task to be run synchronously.  JF
        // TODO: Or maybe not?  Seems to work ok for making sync requests...  JF
        public DoWorkEventArgs Run()
        {
            DoWorkEventArgs e = new DoWorkEventArgs( null );
            OnDoWork( e );
            return e;
        }

        // TODO: Not sure how much effect Cancel has on this.  Needs research.  JF
        protected override void OnDoWork( DoWorkEventArgs e )
        {
            try
            {
                HttpWebRequest request = null;
                if( CancellationPending )
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    request = RequestBroker.GetHttpWebRequest();
                    Stream requestStream = null;
                    if( CancellationPending )
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        if( request.Method == Method.POST || request.Method == Method.PUT )
                        {
                            // HACK: For some reason our POST isn't working without 100-Continue status 
                            //  mechanism disabled.  What's going on here?  Are we operating at a low-enough
                            //  level where we would have to implement the 100-Continue ourselves?  JF
                            request.ServicePoint.Expect100Continue = false;
                            requestStream = request.GetRequestStream();
                            // HACK: I'm a little concerned about just casting ContentLength to an int here.  That's kinda scary.  JF
                            requestStream.Write( RequestBroker.Body, 0, (int)RequestBroker.ContentLength );
                            requestStream.Close();
                        }

                        if( CancellationPending )
                        {
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            HttpWebResponse response = null;
                            try
                            {
                                response = (HttpWebResponse)request.GetResponse();
                            }
                            catch( WebException ex )
                            {
                                if( ex.Response != null )
                                {
                                    response = (HttpWebResponse)ex.Response;
                                }
                                else
                                {
                                    // NOTE: Addresses WebExceptionStatus.Timeout and anything else where there wasn't a 
                                    //  meaningful response from the server to hand back to the caller.  JF
                                    throw;
                                }
                            }

                            byte[] responseBytes;
                            using( Stream responseStream = response.GetResponseStream() )
                            {
                                responseBytes = _readBytes( responseStream, (int)response.ContentLength );
                                responseStream.Close();
                                response.Close();
                            }
                            ResponseBroker = new HttpResponseBroker( response, responseBytes );

                            e.Result = ResponseBroker;
                        }
                    }
                }
            }
            catch( Exception ex )
            {
                Spy.Trace( ex );
            }
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        private static byte[] _readBytes( Stream stream, int initialLength )
        {
            // NOTE: Default to 32K
            if( initialLength < 1 )
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[ initialLength ];
            int read = 0;

            int chunk;
            while( ( chunk = stream.Read( buffer, read, buffer.Length - read ) ) > 0 )
            {
                read += chunk;

                // NOTE: At the end of the buffer?  Need to check and see if there's more to read.
                if( read == buffer.Length )
                {
                    int nextByte = stream.ReadByte();

                    if( nextByte == -1 )
                    {
                        return buffer;
                    }

                    // NOTE: More to read from the stream. Resize the buffer, add the next byte and keep reading.  JF
                    byte[] newBuffer = new byte[ buffer.Length * 2 ];
                    Array.Copy( buffer, newBuffer, buffer.Length );
                    newBuffer[ read ] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }

            // NOTE: Buffer is over-sized so we need to copy just the used portion into a new buffer of the correct size.  JF
            byte[] bytes = new byte[ read ];
            Array.Copy( buffer, bytes, read );
            return bytes;
        }
    }
}
