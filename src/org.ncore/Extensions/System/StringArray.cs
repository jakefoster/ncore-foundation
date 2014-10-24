using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.ncore.Extensions
{
    public static class System_StringArray
    {
        public static string Tokenize( this String[] instance, string delimiter, string encapsulator )
        {
            if( instance == null || instance.Length == 0 )
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            for( int i = 0; i < instance.Length; i++ )
            {
                builder.Append( encapsulator );
                builder.Append( instance[ i ] );
                builder.Append( encapsulator );
                if( i != ( instance.Length - 1 ) )
                {
                    builder.Append( delimiter );
                }
            }
            return builder.ToString();
        }
    }
}
