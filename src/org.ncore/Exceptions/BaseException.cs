using System;

namespace org.ncore.Exceptions 
{
	// TODO: Add xml comments for class
	public abstract class BaseException : ApplicationException
	{
		public const string DEFAULT_MESSAGE = "An exception has been thrown.";
		public const string DEFAULT_INSTRUCTIONS = "The application encountered an unexpected condition and was unable to complete the requested action.";

		/// <summary>
		/// Constructor.
		/// </summary>
		public BaseException() : base( DEFAULT_MESSAGE )
		{
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public BaseException( Exception innerException ) : base( DEFAULT_MESSAGE, innerException )
        {
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">A string containing a debug message that describes the exception.</param>
		public BaseException( string message ) : base( message )
		{
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">A string containing a debug message that describes the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public BaseException( string message, Exception innerException ) : base( message, innerException )
        {
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">A string containing a debug message that describes the exception.</param>
		/// <param name="message">A string containing instruction text to be displayed to the end user.</param>
		public BaseException( string message, string instructions ) : base( message )
		{
			_instructions = instructions;
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">A string containing a debug message that describes the exception.</param>
        /// <param name="instructions">A string containing instruction text to be displayed to the end user.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public BaseException( string message, string instructions, Exception innerException ) : base( message, innerException )
        {
            _instructions = instructions;
        }

        protected string _instructions = DEFAULT_INSTRUCTIONS;
		/// <summary>
		/// Gets a string containing instruction expression to be displayed to the end user.
		/// </summary>
		public virtual string Instructions
		{
			get{ return _instructions; }
		}
	}
}