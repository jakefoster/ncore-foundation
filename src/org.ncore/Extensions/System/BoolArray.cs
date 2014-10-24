using System;
using System.Text;

namespace org.ncore.Extensions
{
    public static class System_BoolArray
    {
        // TODO: XML comments & tests.  JF
        public static byte ToByte( this bool[] instance )
        {
            byte value = 0;

            for( int i = 0; i < 8; i++ )
            {
                if( instance[ i ] == true )
                {
                    value = (byte)( value | (byte)Math.Pow( 2, i ) );
                }
            }

            return value;
        }

        // NOTE: Fill( false ) is useless because a new bool[] is already initialized this way
        //  but Fill( true ) is handy.  JF
        public static bool[] Fill( this bool[] instance, bool value )
        {
            for( int i = 0; i < instance.Length; i++ )
            {
                instance[ i ] = value;
            }
            return instance;
        }

        public static String ToText( this bool[] instance )
        {
            return ToText( instance, true );
        }

        public static String ToText( this bool[] instance, bool littleEndian )
        {
            StringBuilder builder = new StringBuilder( instance.Length );

            if( littleEndian )
            {
                for( int i = instance.Length - 1; i >= 0; i-- )
                {
                    builder.Append( instance[ i ] ? "1" : "0" );
                }
            }
            else
            {
                for( int i = 0; i < instance.Length; i++ )
                {
                    builder.Append( instance[ i ] ? "1" : "0" );
                }                
            }

            return builder.ToString();
        }

        // TODO: Assumes little-endian.  Should we have an overload that allows caller to specify?  JF
        public static UInt32 ToUInt32( this bool[] instance )
        {
            UInt32 value = 0;

            for( int i = 0; i < 32; i++ )
            {
                if( instance[ i ] == true )
                {
                    value = (UInt32)( value | (UInt32)Math.Pow( 2, i ) );
                }
            }

            return value;
        }

        // TODO: Not really sure that I like this on BoolArray.  It is very useful for simulating binary 
        //  operations but I'm not really certain that it's directly related to a bool[].  Maybe needs
        //  it's own class?  Although now that I'm looking at the other extension methods on this type
        //  I'm realizing that they're almost all exclusively about binary represenations.  Hmm...  JF
        public static void SetBit( this bool[] instance, UInt32 bit, bool value )
        {
            instance[ (int)Math.Log( bit, 2 ) ] = value;
        }
    }
}
