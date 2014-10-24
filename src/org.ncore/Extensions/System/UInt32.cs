using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.ncore.Extensions
{
    public static class System_UInt32
    {
        public static bool[] ToBitArray( this UInt32 value )
        {
            bool[] bits = new bool[ 32 ];

            for( int i = 0; i < bits.Length; i++ )
            {
                UInt32 current = (UInt32)Math.Pow( 2, i );
                bits[ i ] = ( value & current ) == current;
            }

            return bits;
        }
    }
}
