using System;
using System.Text;
using System.Web;
using org.ncore.Common;

namespace org.ncore.Extensions
{
    // TODO: Unit tests.  JF
    // TODO: Finish porting over StringEx (and others) to extension methods.  JF
    public static class System_String
    {
        /// <summary>
        /// Converts the string to "short date" (mm/d/yyyy) format.  
        /// If the input is null or empty the method returns an empty string.  
        /// If the input is not a valid date/time a FormatException will be thrown.
        /// </summary>
        /// <returns>The supplied string in the "short date" format.</returns>
        public static string ToShortDateString( this String instance )
        {
            instance = instance.NullToEmpty();
            if( instance != string.Empty )
            {
                return Convert.ToDateTime( instance ).ToShortDateString();
            }
            else
            {
                return instance;
            }
        }

        /// <summary>
        /// Converts the string to "standard date" (MMM dd, yyyy) format.  
        /// If the input is null or empty the method returns an empty string.  
        /// If the input is not a valid date/time a FormatException will be thrown.
        /// </summary>
        /// <returns>The supplied string in the "standard date" format.</returns>
        public static string ToStandardDateString( this string instance )
        {
            instance = instance.NullToEmpty();
            if( instance != string.Empty )
            {
                return Convert.ToDateTime( instance ).ToString( "MMM dd, yyyy" );
            }
            else
            {
                return instance;
            }
        }

        /// <summary>
        /// Converts the string to "standard date/time" (MMM dd, yyyy h:mm AM/PM) format.  
        /// If the input is null or empty the method returns an empty string.  
        /// If the input is not a valid date/time a FormatException will be thrown.
        /// </summary>
        /// <returns>The supplied string in the "standard date/time" format.</returns>
        public static string ToStandardDateTimeString( this string instance )
        {
            instance = instance.NullToEmpty();
            if( instance != string.Empty )
            {
                return Convert.ToDateTime( instance ).ToString( "MMM dd, yyyy h:mm tt" );
            }
            else
            {
                return instance;
            }
        }

        /// <summary>
        /// Converts the string to "long date" (MMMM dd, yyyy) format.  
        /// If the input is null or empty the method returns an empty string.  
        /// If the input is not a valid date/time a FormatException will be thrown.
        /// </summary>
        /// <returns>The supplied string in the "long date" format.</returns>
        public static string ToLongDateString( this string instance )
        {
            instance = instance.NullToEmpty();
            if( instance != string.Empty )
            {
                return Convert.ToDateTime( instance ).ToString( "MMMM dd, yyyy" );
            }
            else
            {
                return instance;
            }
        }

        /// <summary>
        /// Converts the string to "long date/time" (MMMM dd, yyyy h:mm AM/PM) format.  
        /// If the input is null or empty the method returns an empty string.  
        /// If the input is not a valid date/time a FormatException will be thrown.
        /// </summary>
        /// <returns>The supplied string in the "long date/time" format.</returns>
        public static string ToLongDateTimeString( this string instance )
        {
            instance = instance.NullToEmpty();
            if( instance != string.Empty )
            {
                return Convert.ToDateTime( instance ).ToString( "MMMM dd, yyyy h:mm tt" );
            }
            else
            {
                return instance;
            }
        }

        /// <summary>
        /// Evaluates the string and returns string.Empty if the supplied string is null.  Otherwise, returns the input value.
        /// </summary>
        /// <returns>The supplied value if non-null, or string.Empty.</returns>
        public static string NullToEmpty( this string instance )
        {
            if( instance == null )
            {
                return string.Empty;
            }
            return instance;
        }



        /// <summary>
        /// Evaluates the string and returns true if it is empty.  If null or non-empty, returns false.
        /// </summary>
        /// <returns>True if the supplied value is empty.  False if null or non-empty.</returns>
        public static bool IsEmpty( this string instance )
        {
            if( instance == null || instance.Trim() != string.Empty )
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Evaluates the string and returns true if it is empty or null.  If non-null and non-empty, returns false.
        /// </summary>
        /// <returns>True if the supplied value is empty or null.  False if non-null and non-empty.</returns>
        public static bool IsEmptyOrNull( this string instance )
        {
            if( instance == null || instance.IsEmpty() == true )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Evaluates the string and returns true if it is a valid date/time.
        /// </summary>
        /// <returns>True if the supplied value can be converted to a DateTime.</returns>
        public static bool IsDateTime( this string instance )
        {
            try
            {
                DateTime.Parse( instance );
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Inserts the specified number of tabs at the begining of the string.  If the string is null the result will be a string containing only tab characters.
        /// </summary>
        /// <param name="tabsToInsert">The number of tab characters to insert.</param>
        /// <returns></returns>
        public static string Tabify( this string instance, int tabsToInsert )
        {
            if( instance == null )
            {
                instance = string.Empty;
            }

            if( tabsToInsert < 1 )
            {
                return instance;
            }
            else
            {
                return instance.Prepend( '\x0009', tabsToInsert );
            }
        }

        /// <summary>
        /// Places the string inside of single quotes.  If the input is null or empty then the string '' is returned.
        /// </summary>
        /// <returns>The supplied value inside of single quotes or the string '' if the input is null or empty.</returns>
        public static string AddQuotes( this string instance )
        {
            if( instance.IsEmptyOrNull() == true )
            {
                return "''";
            }
            else
            {
                StringBuilder builder = new StringBuilder( instance.Length + 2 );
                builder.Append( "'" );
                builder.Append( instance );
                builder.Append( "'" );
                return builder.ToString();
            }
        }

        /// <summary>
        /// Places the string inside of double quotes.  If the input is null or empty then the string "" is returned.
        /// </summary>
        /// <returns>The string inside of double quotes or the string "" if the input is null or empty.</returns>
        public static string AddDoubleQuotes( this string instance )
        {
            if( instance.IsEmptyOrNull() == true )
            {
                return "\"\"";
            }
            else
            {
                StringBuilder builder = new StringBuilder( instance.Length + 2 );
                builder.Append( "\"" );
                builder.Append( instance.ToString() );
                builder.Append( "\"" );
                return builder.ToString();
            }
        }

        /// <summary>
        /// Converts the string to a base 64 encoded string.  If the input is null or empty the return value will be empty.
        /// </summary>
        /// <returns>The string in base 64.</returns>
        public static string ToBase64( this string instance )
        {
            return instance.ToBase64( Encoding.ASCII, false );
        }

        /// <summary>
        /// Converts the string to a base 64 encoded string, and optionally Url encodes the result.  If the input is null or empty the return value will be empty.
        /// </summary>
        /// <param name="urlEncode">A boolean indicating whether the resulting base 64 string should be Url encoded.</param>
        /// <returns>The string in base 64.</returns>
        public static string ToBase64( this string instance, bool urlEncode )
        {
            return instance.ToBase64( Encoding.ASCII, urlEncode );
        }

        /// <summary>
        /// Converts the string to a base 64 encoded string, and optionally Url encodes the result.  If the input is null or empty the return value will be empty.
        /// </summary>
        /// <param name="encoding">The expression encoding to use for the target string.</param>
        /// <returns>The string in base 64.</returns>
        public static string ToBase64( this string instance, Encoding encoding )
        {
            return instance.ToBase64( encoding, false );
        }

        /// <summary>
        /// Converts the string to a base 64 encoded string, and optionally Url encodes the result.  If the input is null or empty the return value will be empty.
        /// </summary>
        /// <param name="urlEncode">A boolean indicating whether the resulting base 64 string should be Url encoded.</param>
        /// <returns>The string in base 64.</returns>
        public static string ToBase64( this string instance, Encoding encoding, bool urlEncode )
        {
            string encoded = instance.NullToEmpty();
            if( encoded != string.Empty )
            {
                byte[] byteArray = encoding.GetBytes( encoded );
                encoded = Convert.ToBase64String( byteArray );
                if( urlEncode == true )
                {
                    encoded = Uri.EscapeDataString( encoded );
                }
            }
            return encoded;
        }


        /// <summary>
        /// Un-encodes the a 64 string.  If the input is null or empty the return value will be empty.
        /// </summary>
        /// <returns>The un-encoded base 64 string.</returns>
        public static string FromBase64( this string instance )
        {
            return instance.FromBase64( Encoding.ASCII, false );
        }

        /// <summary>
        /// Un-encodes a base 64 string, first Url decoding it specified.  If the input is null or empty the return value will be empty.
        /// </summary>
        /// <param name="urlEncode">A boolean indicating whether the base 64 string should be Url decoded before base 64 un-encoding.</param>
        /// <returns>The un-encoded base 64 string.</returns>
        public static string FromBase64( this string instance, bool urlDecode )
        {
            return instance.FromBase64( Encoding.ASCII, urlDecode );
        }

                /// <summary>
        /// Un-encodes a base 64 string, first Url decoding it specified.  If the input is null or empty the return value will be empty.
        /// </summary>
        /// <param name="urlEncode">A boolean indicating whether the base 64 string should be Url decoded before base 64 un-encoding.</param>
        /// <returns>The un-encoded base 64 string.</returns>
        public static string FromBase64( this string instance, Encoding encoding )
        {
            return instance.FromBase64( encoding, false );
        }

        /// <summary>
        /// Un-encodes a base 64 string, first Url decoding it specified.  If the input is null or empty the return value will be empty.
        /// </summary>
        /// <param name="urlEncode">A boolean indicating whether the base 64 string should be Url decoded before base 64 un-encoding.</param>
        /// <returns>The un-encoded base 64 string.</returns>
        public static string FromBase64( this string instance, Encoding encoding, bool urlDecode )
        {
            string unencoded = instance.NullToEmpty();
            if( unencoded != string.Empty )
            {
                if( urlDecode == true )
                {
                    unencoded = Uri.UnescapeDataString( unencoded );
                }
                byte[] byteArray = Convert.FromBase64String( unencoded );
                unencoded = encoding.GetString( byteArray );
            }
            return unencoded;
        }

        // TODO: Deal with variable encodings.  Right now this (wrongly) assumes .NET strings (UTF-16).  JF
        /// <summary>
        /// Converts the string to a byte array.  If the input is null or empty the return value will be a zero length byte array.
        /// </summary>
        /// <returns>A byte array containing the byte values of the supplied string.</returns>
        public static byte[] ToByteArray( this string instance )
        {
            return ToByteArray( instance, Encoding.Unicode );
        }

        public static byte[] ToByteArray( this string instance, Encoding encoding )
        {
            return encoding.GetBytes( instance );
        }

        /// <summary>
        /// Limits the size of the string to the specified length.  
        /// If the size supplied string is less than the supplied length then the orignial is returned.  
        /// If the input is null or empty the return value will be empty.
        /// </summary>
        /// <param name="maxLength">The maximum allowed length of the string.  Characters beyond this point will be truncated from the string.</param>
        /// <returns>The length limited string or empty if the supplied string is null or empty.</returns>
        // TODO: We should really obsolete this method and replace it with TruncateLeft and TruncateRight methods.  JKF
        public static string LimitLength( this string instance, int maxLength )
        {
            Condition.Assert( maxLength >= 0, "The length parameter must be greater than 0." );

            string buffer = instance.NullToEmpty();
            if( buffer.Length > maxLength )
            {
                return buffer.Substring( 0, maxLength );
            }
            else
            {
                return buffer;
            }
        }


        /// <summary>
        /// Reverses the string value.  If the input is empty or null the return value will be empty.
        /// </summary>
        /// <returns>A string containing the reversed input.  If empty or null an empty string will be returned.</returns>
        public static string Reverse( this string instance )
        {
            if( instance.IsEmptyOrNull() == true )
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder( instance.Length );
            for( int i = instance.Length; i != 0; i-- )
            {
                builder.Append( instance.Substring( i - 1, 1 ) );
            }
            return builder.ToString();
        }

        /// <summary>
        /// Appends the supplied string to the end of the current string, separating the two with the supplied delimiter.
        /// Automatically corrects for the current string ending with the delimiter (it will NOT double delimit).
        /// </summary>
        /// <param name="delimiter">The delimiter to use.</param>
        /// <param name="value">The string to be appended.</param>
        /// <returns></returns>
        public static string AppendDelimited( this string instance, string delimiter, string value )
        {
            StringBuilder builder = new StringBuilder( instance.Length + delimiter.Length + value.Length );

            builder.Append( instance );
            if( instance.EndsWith( delimiter ) == false )
            {
                builder.Append( delimiter );
            }
            builder.Append( value );

            return builder.ToString();
        }

        // TODO: Change to Int32, etc...  JF
        public static bool IsInt( this string instance )
        {
            int i;
            return int.TryParse( instance, out i );
        }

        public static int ToInt( this string instance )
        {
            return int.Parse( instance.Trim() );
        }

        public static bool IsLong( this string instance )
        {
            long l;
            return long.TryParse( instance, out l );
        }

        public static long ToLong( this string instance )
        {
            return long.Parse( instance.Trim() );
        }

        public static bool IsGuid( this string instance )
        {
            try
            {
                Guid guid = new Guid( instance );
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Guid ToGuid( this string instance )
        {
            return new Guid( instance ); ;
        }

        /// <summary>
        /// Prepends the string with the specified number of characters.  If count is 0 or a negative number no characters are prepended.
        /// </summary>
        /// <param name="count">The number of characters to prepend.</param>
        /// <returns>A string containing the specified number of characters prepended.</returns>
        public static String Prepend( this string instance, char value, int repeatCount )
        {
            if( repeatCount < 1 )
            {
                return instance;
            }
            StringBuilder builder = new StringBuilder( instance.Length + repeatCount );
            builder.Append( value, repeatCount );
            builder.Append( instance );
            return builder.ToString();
        }

        public static String Append( this string instance, char value, int repeatCount )
        {
            if( repeatCount < 1 )
            {
                return instance;
            }
            StringBuilder builder = new StringBuilder( instance.Length + repeatCount );
            builder.Append( instance ); 
            builder.Append( value, repeatCount );
            return builder.ToString();
        }

        /// <summary>
        /// Repeats the specified string to the specified length.
        /// </summary>
        /// <param name="length">The length of the string to be returned.</param>
        /// <returns>A string containing a repetition of the specified value to the specified length.</returns>
        public static String Fill( this String instance, string value, int length )
        {
            if( value == null )
            {
                return null;
            }

            if( value == string.Empty || length < 1 )
            {
                return string.Empty;
            }

            if( value.Length < length )
            {
                int repitions = ( length / value.Length ) + 1;
                StringBuilder builder = new StringBuilder( repitions * value.Length );
                for( int i = 1; i <= repitions; i++ )
                {
                    builder.Append( value );
                }
                return builder.ToString().Substring( 0, length );
            }
            else
            {
                return value.LimitLength( length );
            }
        }
    }
}