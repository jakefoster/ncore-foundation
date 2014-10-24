using System;

namespace org.ncore.Extensions
{
    public static class System_Object
    {
        // NOTE: ALLOWS THIS SORT OF THING:
	    //      foreach( string key in ( (Dictionary<string,string>)metadata.DefaultIfNull( new Dictionary<string,string>() ) ).Keys )
        public static Object DefaultIfNull( this Object instance, Object @default )
        {
            if( instance == null )
            {
                return @default;
            }
            else
            {
                return instance;
            }
        }
    }
}
