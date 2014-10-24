using System;
using System.Diagnostics;

namespace org.ncore.Common
{
	public sealed class Condition
	{
		private Condition()
		{
		}

		public static void Assert( bool assertion )
		{
			Assert( assertion, string.Empty );
		}

		public static void Assert( bool assertion, string message )
		{
			if( assertion == false )
			{
				Fail( message ); 
			}
		}

		public static void Assert( bool assertion, Exception exception )
		{
			if( assertion == false )
			{
				throw exception;
			}
		}

        // NOTE: Important! Do not overload! The number of frames to backtrack is hardcoded in here!
        public static void AssertCaller( params string[] allowedCallers )
        {
            StackTrace stackTrace = new StackTrace( 2, false );
            StackFrame callerFrame = stackTrace.GetFrame( 0 );
            string callerTypeName = callerFrame.GetMethod().DeclaringType.ToString();
            string callerName = callerTypeName + "::" + callerFrame.GetMethod().Name;
            
            for( int i = 0; i < allowedCallers.Length; i++ )
            {
                string testCaller = allowedCallers[i];
                if( testCaller.EndsWith( "*" ) )
                {
                    string wildcard = testCaller.TrimEnd( '*' );
                    if( callerName.StartsWith( wildcard ) )
                    {
                        return;
                    }
                }
                else
                {
                    if( testCaller == callerName || testCaller == callerTypeName )
                    {
                        return;
                    }
                }
            }
            throw new AssertionException( "Condition.AssertCaller() failed.  The target method is not accessible by the calling method." );
        }

        // NOTE: Important! Do not overload! The number of frames to backtrack is hardcoded in here!
        public static void AssertNotCaller( params string[] disallowedCallers )
        {
            StackTrace stackTrace = new StackTrace( 2, false );
            StackFrame callerFrame = stackTrace.GetFrame( 0 );
            string callerTypeName = callerFrame.GetMethod().DeclaringType.ToString();
            string callerName = callerTypeName + "::" + callerFrame.GetMethod().Name;

            string exceptionMessage =
                "Condition.AssertNotCaller() failed.  The target method is not accessible by the calling method.";
            for( int i = 0; i < disallowedCallers.Length; i++ )
            {
                string testCaller = disallowedCallers[ i ];
                if( testCaller.EndsWith( "*" ) )
                {
                    string wildcard = testCaller.TrimEnd( '*' );
                    if( callerName.StartsWith( wildcard ) )
                    {
                        throw new AssertionException( exceptionMessage );
                    }
                }
                else
                {
                    if( testCaller == callerName || testCaller == callerTypeName )
                    {
                        throw new AssertionException( exceptionMessage );
                    }                    
                }
            }
            return;
        }

		public static void Fail()
		{
			Fail( null );
		}

		public static void Fail( string message )
		{
			AssertionException exception;
			if( message == null )
			{
				exception = new AssertionException();
			}
			else
			{
				exception = new AssertionException( message );
			}
			throw exception; 	
		}
	}
}
