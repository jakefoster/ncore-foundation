using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace org.ncore.Common
{
	public sealed class EmbeddedResource
	{
        public static Stream LoadAsStream( string qualifiedResourceName, Assembly containingAssembly ) 
		{
			ResourceContainer resourceContainer = new ResourceContainer( qualifiedResourceName, containingAssembly );
			return _loadAsStream( resourceContainer ); 
		}

        public static string LoadAsString( string qualifiedResourceName, Assembly containingAssembly ) 
		{
            ResourceContainer resourceContainer = new ResourceContainer( qualifiedResourceName, containingAssembly );
			return _loadAsString( resourceContainer );
		}

        public static XmlDocument LoadAsXmlDocument( string qualifiedResourceName, Assembly containingAssembly ) 
		{
            ResourceContainer resourceContainer = new ResourceContainer( qualifiedResourceName, containingAssembly );
			return _loadAsXmlDocument( resourceContainer );
		}

        public static XDocument LoadAsXDocument( string qualifiedResourceName, Assembly containingAssembly ) 
        {
            ResourceContainer resourceContainer = new ResourceContainer( qualifiedResourceName, containingAssembly );
            return _loadAsXDocument( resourceContainer );
        }

        public static Icon LoadAsIcon( string qualifiedResourceName, Assembly containingAssembly ) 
		{
            ResourceContainer resourceContainer = new ResourceContainer( qualifiedResourceName, containingAssembly );
			return _loadAsIcon( resourceContainer );
		}

        public static Bitmap LoadAsBitmap( string qualifiedResourceName, Assembly containingAssembly ) 
		{
			ResourceContainer resourceContainer = new ResourceContainer( qualifiedResourceName, containingAssembly );
			return _loadAsBitmap( resourceContainer );
		}

        public static Stream LoadAsStream( string unqualifiedResourceName )
        {
            ResourceContainer resourceContainer = _getResourceContainer( unqualifiedResourceName );
            return _loadAsStream( resourceContainer );
        }

        public static string LoadAsString( string unqualifiedResourceName )
        {
            ResourceContainer resourceContainer = _getResourceContainer( unqualifiedResourceName );
            return _loadAsString( resourceContainer );
        }

        public static XmlDocument LoadAsXmlDocument( string unqualifiedResourceName )
        {
            ResourceContainer resourceContainer = _getResourceContainer( unqualifiedResourceName );
            return _loadAsXmlDocument( resourceContainer );
        }

        public static XDocument LoadAsXDocument( string unqualifiedResourceName )
        {
            ResourceContainer resourceContainer = _getResourceContainer( unqualifiedResourceName );
            return _loadAsXDocument( resourceContainer );
        }

        public static Icon LoadAsIcon( string unqualifiedResourceName )
        {
            ResourceContainer resourceContainer = _getResourceContainer( unqualifiedResourceName );
            return _loadAsIcon( resourceContainer );
        }

        public static Bitmap LoadAsBitmap( string unqualifiedResourceName )
        {
            ResourceContainer resourceContainer = _getResourceContainer( unqualifiedResourceName );
            return _loadAsBitmap( resourceContainer );
        }

        // NOTE: This method (dangerously) assumes that the caller is exactly 2 stack frames above it.
        //  Callers of this method must ensure that their caller (containing the desired resource)
        //  immediately preceeds them.  JF
		private static ResourceContainer _getResourceContainer( string unqualifiedResourceName )
		{
			StackFrame callingStackFrame = new System.Diagnostics.StackFrame(2, false);
			Assembly assembly = callingStackFrame.GetMethod().DeclaringType.Assembly;
			string callingAssembly = assembly.FullName;
			string qualifiedResourceName = callingAssembly.Substring( 0, callingAssembly.IndexOf( ',' ) ) + "." + unqualifiedResourceName;
			return new ResourceContainer( qualifiedResourceName, assembly );
		}

		private static Stream _loadAsStream( ResourceContainer resourceContainer )  
		{
			Stream resourceStream = resourceContainer.ContainingAssembly.GetManifestResourceStream( resourceContainer.QualifiedResourceName );

			if( resourceStream == null )
			{
				throw new MissingResourceException();
			}
			else
			{
				return resourceStream;
			}
		}

		private static string _loadAsString( ResourceContainer resourceContainer )
		{
			Stream stream = _loadAsStream( resourceContainer );
			StreamReader streamReader = new StreamReader( stream );
			return streamReader.ReadToEnd();
		}

		private static XmlDocument _loadAsXmlDocument( ResourceContainer resourceContainer )
		{
			Stream stream = _loadAsStream( resourceContainer );
			StreamReader streamReader = new StreamReader( stream );
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml( streamReader.ReadToEnd() );
			return xmlDocument;
		}

        private static XDocument _loadAsXDocument( ResourceContainer resourceContainer )
        {
            Stream stream = _loadAsStream( resourceContainer );
            StreamReader streamReader = new StreamReader( stream );
            XDocument xDocument = XDocument.Parse( streamReader.ReadToEnd() );
            return xDocument;
        }

		private static Icon _loadAsIcon( ResourceContainer resourceContainer )
		{
			Stream stream = _loadAsStream( resourceContainer );
			return new Icon( stream );
		}

		private static Bitmap _loadAsBitmap( ResourceContainer resourceContainer ) 
		{
			Stream stream = _loadAsStream( resourceContainer );
			return new Bitmap( stream );
		}

		private struct ResourceContainer
		{
			internal readonly string QualifiedResourceName;
			internal readonly Assembly ContainingAssembly;

			internal ResourceContainer( string qualifiedResourceName, Assembly containingAssembly )
			{
				QualifiedResourceName = qualifiedResourceName;
				ContainingAssembly = containingAssembly;
			}
		}
	}
}


