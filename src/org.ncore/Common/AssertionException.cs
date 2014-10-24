using System;

namespace org.ncore.Common
{
    public class AssertionException : ApplicationException
    {
        private const string DEFAULT_MESSAGE = "A condition required by the calling method was not met.  The specified assertion failed.";

        public AssertionException() : base( DEFAULT_MESSAGE ) { }
        public AssertionException( string message ) : base( message ) { }
    }
}
