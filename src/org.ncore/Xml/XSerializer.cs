using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using org.ncore.Common;
using org.ncore.Extensions;
using org.ncore.Text;

namespace org.ncore.Xml
{
    public class XSerializer
    {
        // TODO: Might want to move away from the static methods and make this a full-blown instantiable class.  JF
        // TODO: Hard-code the encoding?  JF
        private static readonly Encoding _encoding = Encoding.UTF8;

        // TODO: Don't know that I like having .SerializeToString() be
        //  the "worker" method.  Isn't this kind of jumping through
        //  hoops to always serialize to a string?  Can't I write it
        //  to XmlDocument directly?  JF
        public static XDocument SerializeToXDocument( object target )
        {
            return XDocument.Parse( SerializeToString( target ) );
        }

        public static XmlDocument SerializeToXmlDocument( object target )
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml( SerializeToString( target ) );
            return document;
        }

        public static string SerializeToString( object target )
        {
            EncodedStringWriter writer = new EncodedStringWriter( _encoding );
            XmlSerializer serializer = new XmlSerializer( target.GetType() );
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = _encoding;

            XmlWriter xmlWriter = XmlWriter.Create( writer, settings );
            serializer.Serialize( xmlWriter, target );
            
            return writer.ToString();
        }

        // TODO: Once again, not sure I like the string version being the
        //  worker.  A lot of back and forth here.  JF
        public static object Deserialize( XDocument xml, Type targetType )
        {
            return Deserialize( xml.ToText( _encoding ), targetType );
        }

        public static object Deserialize( XmlDocument xml, Type targetType )
        {
            EncodedStringWriter writer = new EncodedStringWriter( Encoding.UTF8 );
            XmlTextWriter xmlWriter = new XmlTextWriter( writer );
            xml.WriteTo( xmlWriter );
            return Deserialize( writer.ToString(), targetType );
        }

        public static object Deserialize( string xml, Type targetType )
        {
            MemoryStream memoryStream = new MemoryStream( xml.ToByteArray( _encoding ) );
            XmlTextWriter xmlTextWriter = new XmlTextWriter( memoryStream, _encoding );
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = _encoding;
            XmlWriter xmlWriter = XmlWriter.Create( memoryStream, settings );

            XmlSerializer serializer = new XmlSerializer( targetType );
            return serializer.Deserialize( memoryStream );
        }
    }
}

