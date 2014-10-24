using System;

namespace org.ncore.Common
{
    public class MemberDataTypeMissmatchException : ApplicationException
    {
        private const string DEFAULT_MESSAGE = "The source and target data types for the mapped member are not the same.";

        // UNDONE: Constructor that accepts and prints specified source and target member data types?
        public MemberDataTypeMissmatchException() : base( DEFAULT_MESSAGE ) { }
        public MemberDataTypeMissmatchException( string memberName ) : base( DEFAULT_MESSAGE + " (Member Name: " + memberName + ")" ) { }
    }
}
