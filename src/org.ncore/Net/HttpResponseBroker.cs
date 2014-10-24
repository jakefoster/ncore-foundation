using System;
using System.Net;
using System.Text;

namespace org.ncore.Net
{
    public class HttpResponseBroker
    {
        private string _characterSet;
        private string _contentEncoding;
        private long _contentLength;
        private string _contentType;
        private CookieCollection _cookies = new CookieCollection();
        private WebHeaderCollection _headers = new WebHeaderCollection();
        private bool _isFromCache;
        private bool _isMutuallyAuthenticated;
        private DateTime _lastModified = DateTime.MinValue;
        private string _method = string.Empty;
        private Version _protocolVersion = new Version();
        private Uri _uri = null;
        private string _server = string.Empty;
        private HttpStatusCode _statusCode = HttpStatusCode.Ambiguous;
        private string _statusDescription = string.Empty;
        private byte[] _body;

        public string CharacterSet
        {
            get { return _characterSet; }
        }

        public string ContentEncoding
        {
            get { return _contentEncoding; }
        }

        public long ContentLength
        {
            get { return _contentLength; }
        }

        public string ContentType
        {
            get { return _contentType; }
        }

        public CookieCollection Cookies
        {
            get { return _cookies; }
        }

        public Encoding Encoding
        {
            get 
            {
                if( _contentEncoding != null && _contentEncoding != string.Empty )
                {
                    return Encoding.GetEncoding( _contentEncoding, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback );
                }
                else
                {
                    // TODO: Again, not sure if UTF-8 really is the safest default here.  JF
                    return Encoding.UTF8;
                }
            }
        }

        public WebHeaderCollection Headers
        {
            get { return _headers; }
        }

        public bool IsFromCache
        {
            get { return _isFromCache; }
        }

        public bool IsMutuallyAuthenticated
        {
            get { return _isMutuallyAuthenticated; }
        }

        public DateTime LastModified
        {
            get { return _lastModified; }
        }

        public string Method
        {
            get { return _method; }
        }

        public Version ProtocolVersion
        {
            get { return _protocolVersion; }
        }

        public Uri Uri
        {
            get { return _uri; }
        }

        public string Server
        {
            get { return _server; }
        }

        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
        }

        public string StatusDescription
        {
            get { return _statusDescription; }
        }

        public byte[] Body
        {
            get { return _body; }
        }

        public HttpResponseBroker( WebResponse response ) :
            this( response, new byte[] { } )
        {

        }

        public HttpResponseBroker( WebResponse webResponse, byte[] responseBody )
        {
            // TODO: This whole thing is a bit manky.  Mostly because it could be
            //  a WebResponse, or a derived, HttpWebResponse that gets passed in.  JF
            _contentLength = webResponse.ContentLength;
            _contentType = webResponse.ContentType;

            // NOTE: This is weird, but we always want to set the content type and encoding if we can
            //  so we're going to assume that we *must* do it from _contentType.  It'll get overriden
            //  automatically later in the method if that's not true.  JF
            string inferedEncoding = _inferEncodingFromContentType( webResponse.ContentType );
            _contentEncoding = inferedEncoding;
            _characterSet = inferedEncoding;

            foreach( string key in webResponse.Headers.AllKeys )
            {
                _headers.Add( key, webResponse.Headers[ key ] );
            }

            _isFromCache = webResponse.IsFromCache;
            _isMutuallyAuthenticated = webResponse.IsMutuallyAuthenticated;

            HttpWebResponse httpWebResponse = webResponse as HttpWebResponse;
            if( httpWebResponse != null )
            {
                _characterSet = httpWebResponse.CharacterSet;

                // HACK: This is manky, but for some reason ContentEncoding is coming back from IIS as empty.  
                //  this is a total hack but I'm going to use the CharacterSet value instead (for now).  JF
                if( httpWebResponse.ContentEncoding != string.Empty )
                {
                    _contentEncoding = httpWebResponse.ContentEncoding;
                }
                else
                {
                    _contentEncoding = httpWebResponse.CharacterSet;
                }

                // HACK: Not sure it's really safe to just reference the requests cookies?!  JF
                _cookies = httpWebResponse.Cookies;
                //foreach( Cookie cookie in httpWebResponse.Cookies )
                //{
                //    Cookie copy = new Cookie();
                //    copy.Comment = cookie.Comment;
                //    copy.CommentUri = cookie.CommentUri;
                //    copy.Discard = cookie.Discard;
                //    copy.Domain = cookie.Domain;
                //    copy.Expired = cookie.Expired;
                //    copy.Expires = cookie.Expires;
                //    copy.HttpOnly = cookie.HttpOnly;
                //    copy.Name = cookie.Name;
                //    copy.Path = cookie.Path;
                //    copy.Port = cookie.Port;
                //    copy.Secure = cookie.Secure;
                //    // HACK: Ugh!  THis property is read-only.  Presumably it gets set on creation or something.  Very weird and annoying.  JF
                //    //copy.TimeStamp = cookie.TimeStamp;
                //    copy.Value = cookie.Value;
                //    copy.Version = cookie.Version;
                //    _cookies.Add( copy );
                //}

                _lastModified = httpWebResponse.LastModified;
                _method = httpWebResponse.Method;
                _protocolVersion = httpWebResponse.ProtocolVersion;
                _uri = httpWebResponse.ResponseUri;
                _server = httpWebResponse.Server;
                _statusCode = httpWebResponse.StatusCode;
                _statusDescription = httpWebResponse.StatusDescription;
            }

            _body = responseBody;
        }

        public string GetBody()
        {
            return this.Encoding.GetString( _body );
        }

        private static string _inferEncodingFromContentType( string contentType )
        {
            string matchText = "charset=";
            int start = contentType.IndexOf( matchText, 0, contentType.Length );

            if( start > -1 )
            {
                int offset = start + matchText.Length;
                // HACK: Assumes that "charset=" is always the last part of contentType.  JF
                string encoding = contentType.Substring( offset, contentType.Length - offset );
                int end = encoding.IndexOf( ';' );
                if( end > -1 )
                {
                    encoding = encoding.Substring( 0, end );
                }
                return encoding;
            }
            else
            {
                // TODO: Or should we default to UTF-8 or something?!  JF
                return string.Empty;
            }
        }
    }
}