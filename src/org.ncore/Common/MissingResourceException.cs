using System;

namespace org.ncore.Common
{
    public class MissingResourceException : ApplicationException
    {
        private const string DEFAULT_MESSAGE = "The resource does not exist in the specified assembly.";

        // UNDONE: Constructor that accepts and prints specified assembly and resource name?
        public MissingResourceException() : base( DEFAULT_MESSAGE ) { }
        public MissingResourceException( string message ) : base( message ) { }
    }
}
