using System;
using System.IO;
using System.Text;

namespace org.ncore.Text
{
    // TODO: Figure out if I need to support the byte order mark thing.  Right now it doesn't emit BOMs.  
    //  I'm not sure if this is something that should be optional (maybe a bool on the constructor) and 
    //  if so, how it should be implemented.  Needs investigation.  JF
    public class EncodedStringWriter : StringWriter
    {
        private readonly Encoding _encoding;

        public EncodedStringWriter()
            : base() { }

        public EncodedStringWriter( IFormatProvider formatProvider )
            : base( formatProvider ) { }

        public EncodedStringWriter( StringBuilder stringBuilder )
            : base( stringBuilder ) { }

        public EncodedStringWriter( StringBuilder stringBuilder, IFormatProvider formatProvider )
            : base( stringBuilder, formatProvider ) { }

        public EncodedStringWriter( Encoding encoding )
            : base()
        {
            _encoding = encoding;
        }

        public EncodedStringWriter( IFormatProvider formatProvider, Encoding encoding )
            : base( formatProvider )
        {
            _encoding = encoding;
        }

        public EncodedStringWriter( StringBuilder stringBuilder, Encoding encoding )
            : base( stringBuilder )
        {
            _encoding = encoding;
        }

        public EncodedStringWriter( StringBuilder stringBuilder, IFormatProvider formatProvider, Encoding encoding )
            : base( stringBuilder, formatProvider )
        {
            _encoding = encoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return _encoding ?? base.Encoding;
            }
        }
    }
}
