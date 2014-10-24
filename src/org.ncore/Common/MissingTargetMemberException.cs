using System;

namespace org.ncore.Common
{
    public class MissingTargetMemberException : ApplicationException
    {
        private const string DEFAULT_MESSAGE = "The member specified does not exist in the target type.";

        // UNDONE: Constructor that accepts and prints specified member and taget type names?
        public MissingTargetMemberException() : base( DEFAULT_MESSAGE ) { }
        public MissingTargetMemberException( string memberName ) : base( DEFAULT_MESSAGE + " (Member Name: " + memberName + ")" ) { }
    }
}
