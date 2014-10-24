using System;
using System.Xml;
using System.Xml.Linq;

namespace org.ncore.Extensions
{
    public static class System_Xml_Linq_XElement
    {
        public static XmlElement ToXmlElement( this XElement instance )
        {
            XmlReader reader = instance.CreateReader();
            XmlDocument document = new XmlDocument();
            document.Load( reader );
            return document.DocumentElement;
        }

        public static string GetValueOrEmpty( this XElement instance )
        {
            if( instance != null )
            {
                return instance.Value.NullToEmpty();
            }
            return string.Empty;
        }

        public static string GetValueOrNull( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return instance.Value;
            }
            return null;
        }

        public static Boolean? GetValueAsBoolean( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return Boolean.Parse( instance.Value );
            }
            return null;
        }

        public static Byte? GetValueAsByte( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return Byte.Parse( instance.Value );
            }
            return null;
        }

        public static SByte? GetValueAsSByte( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return SByte.Parse( instance.Value );
            }
            return null;
        }

        public static Char? GetValueAsChar( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return Char.Parse( instance.Value );
            }
            return null;
        }

        public static Int16? GetValueAsInt16( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return Int16.Parse( instance.Value );
            }
            return null;
        }

        public static UInt16? GetValueAsUInt16( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return UInt16.Parse( instance.Value );
            }
            return null;
        }

        public static Int32? GetValueAsInt32( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return Int32.Parse( instance.Value );
            }
            return null;
        }

        public static UInt32? GetValueAsUInt32( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return UInt32.Parse( instance.Value );
            }
            return null;
        }

        public static Int64? GetValueAsInt64( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return Int64.Parse( instance.Value );
            }
            return null;
        }

        public static UInt64? GetValueAsUInt64( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return UInt64.Parse( instance.Value );
            }
            return null;
        }

        public static Single? GetValueAsSingle( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return Single.Parse( instance.Value );
            }
            return null;
        }

        public static Double? GetValueAsDouble( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return Double.Parse( instance.Value );
            }
            return null;
        }

        public static Decimal? GetValueAsDecimal( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return Decimal.Parse( instance.Value );
            }
            return null;
        }

        public static DateTime? GetValueAsDateTime( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return DateTime.Parse( instance.Value );
            }
            return null;
        }

        public static Guid? GetValueAsGuid( this XElement instance )
        {
            if( instance != null && instance.Value != string.Empty )
            {
                return Guid.Parse( instance.Value );
            }
            return null;
        }
    }
}
