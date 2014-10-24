using System;

namespace org.ncore.Extensions
{
    public static class System_Byte
    {

        // TODO: XML comments & tests.  JF
        public static bool[] ToBoolArray( this byte instance )
        {
            bool[] bits = new bool[ 8 ];

            for( int i = 0; i < bits.Length; i++ )
            {
                byte current = (byte)Math.Pow( 2, i );
                bits[ i ] = ( instance & current ) == current;
            }

            return bits;
        }
    }
}
