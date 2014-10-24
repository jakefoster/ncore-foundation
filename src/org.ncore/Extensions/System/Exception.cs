using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using org.ncore.Diagnostics;
using org.ncore.Exceptions;

namespace org.ncore.Extensions
{
    public static class System_Exception
    {
        public static XDocument ToXDocument( this Exception instance, string key = "", string action = "", Dictionary<string, string> metadata = null, string @namespace = "http://schemas.ncore.org/Global/Exception/1/" )
        {
            if( metadata == null )
            {
                metadata = new Dictionary<string, string>();
            }
            return _transformExceptionToFault( instance, key, action, metadata, @namespace );
        }

        private static XDocument _transformExceptionToFault( Exception exception, string key, string action, Dictionary<string, string> metadata, XNamespace @namespace )
        {
            const string SCHEMA_REVISION = "1";

            XDocument tree = new XDocument(
                new XElement( @namespace + "ExceptionTree",
                    new XAttribute( "Key", key ),
                    new XAttribute( "Action", action ),
                    new XAttribute( "Revision", SCHEMA_REVISION ),
                    _renderExceptionXml( exception, @namespace ),
                    new XElement( @namespace + "Metadata" )
                    ) );

            foreach( string itemKey in ( (Dictionary<string, string>)metadata.DefaultIfNull( new Dictionary<string, string>() ) ).Keys )
            {
                tree.Root.Element( @namespace + "Metadata" ).Add(
                    new XElement( @namespace + "Item",
                        new XAttribute( "Type", itemKey ),
                        new XText( metadata[ itemKey ] ) ) );
            }

            return tree;
        }

        private static XElement _renderExceptionXml( Exception exception, XNamespace @namespace )
        {
            if( exception == null )
            {
                return null;
            }

            string instructions = string.Empty;
            if( exception is BaseException )
            {
                instructions = ( (BaseException)exception ).Instructions;
            }

            XElement innerExceptionXml = exception.InnerException != null ? _renderExceptionXml( exception.InnerException, @namespace ) : null;

            XElement element = new XElement(
                    new XElement( @namespace + "Exception",
                        new XElement( @namespace + "Type",
                            new XText( exception.GetType().FullName ) ),
                        new XElement( @namespace + "Message",
                            new XText( exception.Message ) ),
                        new XElement( @namespace + "Instructions",
                            new XText( instructions ) ),
                        new XElement( @namespace + "Source",
                            new XText( exception.Source ?? string.Empty ) ),
                        new XElement( @namespace + "Operation",
                            new XText( exception.TargetSite != null ? exception.TargetSite.ToString() : string.Empty ) ),
                        new XElement( @namespace + "StackTrace",
                            new XText( exception.StackTrace != null ? exception.StackTrace.ToString() : string.Empty ) ),
                        new XElement( @namespace + "InnerException",
                            innerExceptionXml ) ) );

            return element;
        }
    }
}
