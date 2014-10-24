using System.Text;
using System.Xml;
using System.Xml.Linq;
using org.ncore.Common;
using org.ncore.Text;

namespace org.ncore.Extensions
{
    public static class System_Xml_Linq_XDocument
    {
        public static byte[] ToByteArray( this XDocument instance, Encoding encoding )
        {
            return instance.ToText( encoding ).ToByteArray( encoding );
        }

        public static byte[] ToByteArray( this XDocument instance, XmlWriterSettings settings )
        {
            return instance.ToText( settings ).ToByteArray( settings.Encoding );
        }

        public static string ToText( this XDocument instance, Encoding encoding )
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = encoding;
            
            return instance.ToText( settings );
        }

        public static string ToText( this XDocument instance, XmlWriterSettings settings )
        {
            EncodedStringWriter writer = new EncodedStringWriter( settings.Encoding );
            using( XmlWriter xw = XmlWriter.Create( writer, settings ) )
            {
                instance.WriteTo( xw );
            }

            return writer.ToString();
        }

        public static XmlDocument ToXmlDocument( this XDocument instance )
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml( instance.ToString() );
            return document;
        }
    }
}
