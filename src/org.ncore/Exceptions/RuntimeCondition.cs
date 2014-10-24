using System;

namespace org.ncore.Exceptions 
{
	/// <summary>
	/// The core exception type for all runtime conditions.  All other conditions should derive from this class.
	/// This branch of the exception tree deals with non-error conditions related to domain logic.
	/// </summary>
	public class RuntimeCondition : BaseException 
	{
		new public const string DEFAULT_MESSAGE = "A runtime condition has occurred.";
		new public const string DEFAULT_INSTRUCTIONS = "The application encountered an unexpected runtime condition requiring user intervention.";

		/// <summary>
		/// Constructor.
		/// </summary>
		public RuntimeCondition() : base( DEFAULT_MESSAGE, DEFAULT_INSTRUCTIONS )
		{
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RuntimeCondition( Exception innerException ) : base( DEFAULT_MESSAGE, DEFAULT_INSTRUCTIONS, innerException )
        {
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">A string containing a debug message that describes the exception.</param>
		public RuntimeCondition( string message ) : base( message, DEFAULT_INSTRUCTIONS )
		{
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">A string containing a debug message that describes the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RuntimeCondition( string message, Exception innerException ) : base( message, DEFAULT_INSTRUCTIONS, innerException )
        {
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">A string containing a debug message that describes the exception.</param>
		/// <param name="instructions">A string containing instruction text to be displayed to the end user.</param>
		public RuntimeCondition( string message, string instructions ) : base( message, instructions )
		{
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">A string containing a debug message that describes the exception.</param>
        /// <param name="instructions">A string containing instruction expression to be displayed to the end user.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RuntimeCondition( string message, string instructions, Exception innerException ) : base( message, instructions, innerException )
        {
        }
	}
}