using System;

public class MissingSourceMemberException : ApplicationException
{
    private const string DEFAULT_MESSAGE = "The member specified does not exist in the source type. ";

    // TODO: Constructor that accepts and prints specified member and source type names?
    public MissingSourceMemberException( string templateMemberName ) : base( DEFAULT_MESSAGE + templateMemberName ) { }
}

