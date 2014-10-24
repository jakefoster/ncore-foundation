using System.Text;

namespace org.ncore.Extensions
{
    // TODO: Unit tests.  JF
    public static class System_ByteArray
    {
        public static string ToText( this byte[] instance )
        {
            return ToText( instance, Encoding.Unicode );
        }

        public static string ToText( this byte[] instance, Encoding encoding )
        {
            return ToText( instance, 0, encoding );
        }

        /// <summary>
        /// Converts the byte array to a string.  
        /// If the array is zero length the return value will be an empty string.
        /// </summary>
        /// <returns>The string containing the converted byte array.</returns>
        public static string ToText( this byte[] instance, int startIndex )
        {
            return ToText( instance, startIndex, instance.Length - startIndex, Encoding.Unicode );
        }

        /// <summary>
        /// Converts the byte array to a string.  
        /// If the array is zero length the return value will be an empty string.
        /// </summary>
        /// <returns>The string containing the converted byte array.</returns>
        public static string ToText( this byte[] instance, int startIndex, Encoding encoding )
        {
            return ToText( instance, startIndex, instance.Length - startIndex, encoding );
        }

        /// <summary>
        /// Converts the specified number of elements in the byte array to a string.  
        /// If the array is zero length the return value will be an empty string.  
        /// If the lengthToConvert is greater than the number of elements in the byte array then the whole byte array will be converted.
        /// </summary>
        /// <param name="lengthToConvert">The number of elements in the byte array to convert.</param>
        /// <returns>The string containing the converted byte array.</returns>
        public static string ToText( this byte[] instance, int startIndex, int numberOfBytes, Encoding encoding )
        {
            string text = encoding.GetString( instance, startIndex, numberOfBytes );
            return text;
        }

        public static string ToPrintable( this byte[] instance, string separator )
        {
            StringBuilder builder = new StringBuilder();
            for( int i = 0; i < instance.Length; i++ )
            {
                builder.Append( instance[ i ].ToString() );
                if( i != ( instance.Length - 1 ) )
                {
                    builder.Append( separator );
                }
            }
            return builder.ToString();
        }
    }
}
