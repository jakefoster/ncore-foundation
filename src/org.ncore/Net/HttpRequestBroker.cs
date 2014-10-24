using System;
using System.Net;
using System.Text;

namespace org.ncore.Net
{
    public class HttpRequestBroker
    {
        private string _accept;
        private bool _allowAutoRedirect = true;
        // TODO: I think contentLength should be drawn from the body length - not user specifiable.  Seems too easy to break.  JF
        //private long _contentLength;
        private string _contentType;
        private Encoding _encoding = Encoding.UTF8; // TODO: Not sure if this is the best default.  JF
        private string _expect;
        private CookieCollection _cookies = new CookieCollection();
        private WebHeaderCollection _headers = new WebHeaderCollection();
        private string _method = HttpBackgroundWorker.Method.GET;
        private Version _protocolVersion = new Version( 1, 1 );
        private Uri _uri;
        private int _timeout = 30000;  // NOTE: The HttpWebRequest has a 100 second timeout by default.  I think this is not cool so I'm overriding it.  JF
        private byte[] _body;

        public string Accept
        {
            get { return _accept; }
            set { _accept = value; }
        }

        public bool AllowAutoRedirect
        {
            get { return _allowAutoRedirect; }
            set { _allowAutoRedirect = value; }
        }

        public long ContentLength
        {
            get
            {
                // TODO: Not sure how this matches up with HttpWebRequest.  JF
                if( _body == null )
                {
                    return -1;
                }
                else
                {
                    return _body.Length;
                }
            }
            //set { _contentLength = value; }
        }

        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        // TODO: Do we need to worry about matching with the encoding implied in ContentType or is it buyer beware?  JF
        public Encoding Encoding
        {
            get{ return _encoding; }
            set { _encoding = value; }
        }

        public string Expect
        {
            get { return _expect; }
            set { _expect = value; }
        }

        public CookieCollection Cookies
        {
            get { return _cookies; }
            set { _cookies = value; }
        }

        public WebHeaderCollection Headers
        {
            get { return _headers; }
            set { _headers = value; }
        }

        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }

        public Version ProtocolVersion
        {
            get { return _protocolVersion; }
            set { _protocolVersion = value; }
        }

        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public Uri Uri
        {
            get { return _uri; }
            set { _uri = value; }
        }

        public byte[] Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public void SetBody( string body )
        {
            _body = _encoding.GetBytes( body );
        }

        public HttpRequestBroker( string url )
        {
            _uri = new Uri( url );
        }

        public HttpRequestBroker( string url, string body )
        {
            _uri = new Uri( url );
            _method = HttpBackgroundWorker.Method.POST;
            this.SetBody( body );
        }

        public HttpRequestBroker( string url, string method, string body )
        {
            _uri = new Uri( url );
            _method = method;
            this.SetBody( body );
        }

        public HttpRequestBroker( string url, Encoding encoding, byte[] body )
        {
            _uri = new Uri( url );
            _method = HttpBackgroundWorker.Method.POST;
            _encoding = encoding;
            _body = body;
        }

        public HttpRequestBroker( string url, string method, Encoding encoding, byte[] body )
        {
            _uri = new Uri( url );
            _method = method;
            _encoding = encoding;
            _body = body;
        }

        public HttpRequestBroker( Uri uri )
        {
            _uri = uri;
        }

        public HttpRequestBroker( Uri uri, string body )
        {
            _uri = uri;
            _method = HttpBackgroundWorker.Method.POST;
            this.SetBody( body );
        }

        public HttpRequestBroker( Uri uri, string method, string body )
        {
            _uri = uri;
            _method = method;
            this.SetBody( body );
        }

        public HttpRequestBroker( Uri uri, Encoding encoding, byte[] body )
        {
            _uri = uri;
            _method = HttpBackgroundWorker.Method.POST;
            _encoding = encoding;
            _body = body;
        }

        public HttpRequestBroker( Uri uri, string method, Encoding encoding, byte[] body )
        {
            _uri = uri;
            _method = method;
            _encoding = encoding;
            _body = body;
        }

        public HttpWebRequest GetHttpWebRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create( _uri );
            request.Accept = _accept;
            //requestBroker.Address -- get only
            request.AllowAutoRedirect = _allowAutoRedirect;
            //requestBroker.AllowWriteStreamBuffering -- ?
            //requestBroker.AuthenticationLevel -- ?
            //requestBroker.AutomaticDecompression -- ?
            //requestBroker.CachePolicy -- ?
            //requestBroker.ClientCertificates -- ?
            //requestBroker.Connection -- ?
            //requestBroker.ConnectionGroupName -- ?
            if( this.ContentLength > 0 )
            {
                request.ContentLength = this.ContentLength;
            }
            request.ContentType = _contentType;
            //requestBroker.ContinueDelegate -- ?
            
            // HACK: Not sure it's safe to reference _cookies?!  JF
            if( _cookies.Count > 0 )
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add( _cookies );
            }

            //requestBroker.Credentials -- ?
            request.Expect = _expect;

            foreach( string key in _headers.AllKeys )
            {
                request.Headers.Add( key, _headers[ key ] );
            }

            //requestBroker.IfModifiedSince -- ?
            //requestBroker.ImpersonationLevel -- ?
            //requestBroker.KeepAlive -- ?
            //requestBroker.MaximumAutomaticRedirections -- ?
            //requestBroker.MaximumResponseHeadersLength -- ?
            //requestBroker.MediaType -- ?
            request.Method = _method;
            //requestBroker.Pipelined -- ?
            //requestBroker.PreAuthenticate -- ?
            request.ProtocolVersion = _protocolVersion;
            //requestBroker.Proxy -- ?
            //requestBroker.ReadWriteTimeout -- ?
            //requestBroker.Referer -- ?
            //requestBroker.RequestUri -- get only
            //requestBroker.SendChunked -- ?
            //requestBroker.ServicePoint -- get only
            request.Timeout = _timeout;
            //requestBroker.TransferEncoding -- ?
            //requestBroker.UnsafeAuthenticatedConnectionSharing -- ?
            //requestBroker.UseDefaultCredentials -- ?
            //requestBroker.UserAgent -- ?

            return request;
        }
    }
}
