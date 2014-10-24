using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using org.ncore.Extensions;
using org.ncore.Exceptions;

namespace org.ncore.Diagnostics
{
    // TODO: Include TextBoxTraceListener and RichTextBoxTraceListener classes in this namespace too?  JF
    // TODO: Need unit tests.  JF
    public static class Spy
    {
        public static bool IsEnabled
        {
            get
            {
                string configString = ConfigurationManager.AppSettings[ "org.ncore.Diagnostics/Spy.IsEnabled" ];
                bool isEnabled = true;
                if( !configString.IsEmptyOrNull() )
                {
                    isEnabled = bool.Parse( configString );
                }
                return isEnabled;
            }
        }

        // TODO: How does this interoperate with CorrelationManager and TraceSource? JF
        //  http://msdn.microsoft.com/en-us/library/system.diagnostics.correlationmanager.aspx
        //  Actually, it kind of looks like it should just work, no?  E.g.:

        // NOTE: VERY IMPORTANT!  Do not optimize any of these overloads!  Because we're backtracking into the stack frame to
        //  get info about the calling method we have to make sure that that's always done directly from the public, called 
        //  method.  Otherwise we'll grab the wrong stack frame and report the wrong class and line number.  See the note on
        //  _getCallerStackFrame().  JF

        // TODO: Implement Spy.DebugLineIf, Spy.DebugIf, Spy.DebugAssert and corresponding Trace... methods.  JF
        public static string Trap<T>( Expression<Func<T>> lambdaExpression )
        {
            if( !(lambdaExpression is LambdaExpression) 
                || !_haveTraceListeners() 
                || !IsEnabled )
            {
                return null;
            }

            string message = _serializeExpression<T>( lambdaExpression );
            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _trap( now, threadInfo, callerInfo, EventClass.Audit, message );
        }

        public static string TrapIf<T>( bool condition, Expression<Func<T>> lambdaExpression )
        {
            if( !condition 
                || !( lambdaExpression is LambdaExpression )
                || !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string message = _serializeExpression<T>( lambdaExpression );
            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _trap( now, threadInfo, callerInfo, EventClass.Audit, message );
        }

        public static string Trap<T>( Expression<Func<T>> lambdaExpression, EventClass eventClass )
        {
            if( !( lambdaExpression is LambdaExpression )
                || !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string message = _serializeExpression<T>( lambdaExpression );
            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _trap( now, threadInfo, callerInfo, eventClass, message );
        }

        public static string TrapIf<T>( bool condition, Expression<Func<T>> lambdaExpression, EventClass eventClass )
        {
            if( !condition 
                || !( lambdaExpression is LambdaExpression ) 
                || !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string message = _serializeExpression<T>( lambdaExpression );
            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _trap( now, threadInfo, callerInfo, eventClass, message );
        }
        
        private static string _trap( string timestamp, string threadInfo, string callerInfo, EventClass eventClass, string message )
        {
            string line = String.Format( "{0} {1}    {2} {3}  {4} #SPY", timestamp, threadInfo, _getEventClassSymbol( eventClass ), message.Replace( Environment.NewLine, @"\r\n" ), callerInfo );
            System.Diagnostics.Trace.WriteLine( line );
            return line;
        }

        private static string _serializeExpression<T>( Expression<Func<T>> lambdaExpression )
        {
            string dynamicName = _findDynamicName<T>( lambdaExpression );
            string expression = lambdaExpression.Body.ToString();
            object value = "???";
            string message = string.Empty;
            if( lambdaExpression.Body is MemberExpression )
            {
                var body = ( (MemberExpression)lambdaExpression.Body );
                if( body.Member is FieldInfo )
                {
                    expression = body.Member.Name;
                    value = ( (FieldInfo)body.Member ).GetValue( ( (ConstantExpression)body.Expression ).Value );
                }
                else if( body.Member is PropertyInfo )
                {
                    if( dynamicName != string.Empty && dynamicName != lambdaExpression.Body.ToString() )
                    {
                        expression = lambdaExpression.Body.ToString().Replace( dynamicName, string.Empty );
                    }
                    value = lambdaExpression.Compile()();
                }
            }
            else
            {
                // TODO: Could support other specific expr.Body types if needed.  JF
                if( dynamicName != string.Empty && dynamicName != lambdaExpression.Body.ToString() )
                {
                    expression = lambdaExpression.Body.ToString().Replace( dynamicName, string.Empty );
                }
                else
                {
                    // TODO: Handles ( () => this ) but I'm not sure that this really is ONLT "this".  Research.  JF
                    expression = "this"; //expression.Replace( "value", "this" );
                }
                value = lambdaExpression.Compile()().ToString();
            }
            string valueType = "?";
            string valueString = "null";
            if( value != null )
            {
                valueType = value.GetType().Name;
                valueString = value.ToString();
            }

            message = String.Format( "{0}: ({1}){2}", expression, valueType, valueString );
            return message;
        }

        private static string _findDynamicName<T>( Expression<Func<T>> expr )
        {
            string dynamicName = string.Empty;
            try
            {
                string body = expr.Body.ToString();
                int start = body.IndexOf( "value" );

                if( start == -1 )
                {
                    // NOTE: Often there is no dynamic name part so this will legitimately return string.Empty in these cases.  JKF
                    return dynamicName;
                }

                string dynTrimLeft = body.Substring( start );
                // TODO: Trying to handle Spy.Trap( () => this ); scenario.  Is this really the best way?
                if( dynTrimLeft.IndexOf( ')' ) + 2 > dynTrimLeft.Length )
                {
                    dynamicName = dynTrimLeft;
                }
                else
                {
                    dynamicName = dynTrimLeft.Substring( 0, dynTrimLeft.IndexOf( ')' ) + 2 );
                }
            }
            catch( Exception )
            {
                // NOTE: Worst case is that we can't determine the dynamic part of the expression and we just use the whole thing.  JF
            }
            return dynamicName;
        }

        public static string Mark( bool includeLineNumber = false )
        {
            if( !_haveTraceListeners() 
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string markText = _formatMarkText( callerFrame, string.Empty, includeLineNumber );
            string threadInfo = _formatThreadInfoText();
            return _mark( now, threadInfo, markText, callerInfo );
        }

        public static string MarkIf( bool condition, bool includeLineNumber = false )
        {
            if( !condition 
                || !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string markText = _formatMarkText( callerFrame, string.Empty, includeLineNumber );
            string threadInfo = _formatThreadInfoText();
            return _mark( now, threadInfo, markText, callerInfo );
        }

        public static string Mark( string label, bool includeLineNumber = false )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string markText = _formatMarkText( callerFrame, label, includeLineNumber );
            string threadInfo = _formatThreadInfoText();
            return _mark( now, threadInfo, markText, callerInfo );
        }

        public static string MarkIf( bool condition, string label, bool includeLineNumber = false )
        {
            if( !condition
                || !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string markText = _formatMarkText( callerFrame, label, includeLineNumber );
            string threadInfo = _formatThreadInfoText();
            return _mark( now, threadInfo, markText, callerInfo );
        }

        private static string _formatMarkText( StackFrame callerFrame, string label, bool includeLineNumber )
        {
            string annotationText = _formatAnnotationText( callerFrame, label, includeLineNumber );
            string method = String.Format( "{0}.{1}(){2}", callerFrame.GetMethod().DeclaringType.Name, callerFrame.GetMethod().Name, annotationText );
            return method;
        }

        private static string _formatAnnotationText( StackFrame callerFrame, string label, bool includeLineNumber )
        {
            string lineNumberText = string.Empty;
            if( includeLineNumber )
            {
                lineNumberText = String.Format( "@@{0}", callerFrame.GetFileLineNumber().ToString( CultureInfo.InvariantCulture ) );
            }

            string labelText = string.Empty;
            if( label != string.Empty )
            {
                labelText = String.Format( "@``{0}``", label );
            }

            string annotationText = string.Empty;
            if( lineNumberText != string.Empty || labelText != string.Empty )
            {
                annotationText = String.Format( "  {0}{1}", labelText, lineNumberText );
            }
            
            return annotationText;
        }

        private static string _mark( string timestamp, string threadInfo, string label, string callerInfo )
        {
            string line = String.Format( "{0} {1}  _________ {2} _________  {3} #SPY", timestamp, threadInfo, label, callerInfo );
            System.Diagnostics.Trace.WriteLine( line );
            return line;
        }

        public static string Trace( string message )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _traceLine( now, threadInfo, callerInfo, EventClass.Audit, message );
        }

        public static string TraceIf( bool condition, string message )
        {
            if( !condition
                || !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _traceLine( now, threadInfo, callerInfo, EventClass.Audit, message );
        }

        public static string Trace( EventClass eventClass, string message )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _traceLine( now, threadInfo, callerInfo, eventClass, message );
        }

        public static string TraceIf( bool condition, EventClass eventClass, string message )
        {
            if( !condition
                || !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _traceLine( now, threadInfo, callerInfo, eventClass, message );
        }

        public static string Trace( string message, params object[] args )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _traceLine( now, threadInfo, callerInfo, EventClass.Audit, String.Format( message, args ) );
        }

        public static string TraceIf( bool condition, string message, params object[] args )
        {
            if( !condition
                || !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _traceLine( now, threadInfo, callerInfo, EventClass.Audit, String.Format( message, args ) );
        }

        public static string Trace( EventClass eventClass, string message, params object[] args )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _traceLine( now, threadInfo, callerInfo, eventClass, String.Format( message, args ) );
        }

        public static string TraceIf( bool condition, EventClass eventClass, string message, params object[] args )
        {
            if( !condition
                || !_haveTraceListeners()
                || !IsEnabled )
            {
                return null;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            return _traceLine( now, threadInfo, callerInfo, eventClass, String.Format( message, args ) );
        }

        private static string _traceLine( string timestamp, string threadInfo, string callerInfo, EventClass eventClass, string message )
        {
            string line = String.Format( "{0} {1}    {2} {3}  {4} #SPY", timestamp, threadInfo, _getEventClassSymbol( eventClass ), message.Replace( Environment.NewLine, @"\r\n" ), callerInfo );
            System.Diagnostics.Trace.WriteLine( line );
            return line;
        }

        public static void Trace( Exception exception )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            _traceException( now, threadInfo, callerInfo, exception );
        }

        private static void _traceException( string timestamp, string threadInfo, string callerInfo, Exception exception )
        {
            System.Diagnostics.Trace.WriteLine( String.Format( "{0} {1}    ! _EXCEPTION_  {2} #SPY", timestamp, threadInfo, callerInfo ) );
            if( exception != null )
            {
                System.Diagnostics.Trace.WriteLine( ">>" );
                System.Diagnostics.Trace.WriteLine( "Exception:" );
                _traceExceptionDetails( exception );
                System.Diagnostics.Trace.WriteLine( "<<" );
            }
        }

        private static void _traceExceptionDetails( Exception exception )
        {
            System.Diagnostics.Trace.Indent();
            Type type = exception.GetType();
            System.Diagnostics.Trace.WriteLine( String.Format( "Type: {0} ({1})", type.Name, type.FullName ) );
            System.Diagnostics.Trace.WriteLine( String.Format( "Message: {0}", exception.Message.Replace( Environment.NewLine, @"\r\n" ) ) );
            var baseException = exception as BaseException;
            if( baseException != null )
            {
                System.Diagnostics.Trace.WriteLine( String.Format( "Instructions: {0}", baseException.Instructions.Replace( Environment.NewLine, @"\r\n" ) ) );
            }
            System.Diagnostics.Trace.WriteLine( String.Format( "Source: {0}", exception.Source ) );
            System.Diagnostics.Trace.WriteLine( String.Format( "TargetSite: {0}", exception.TargetSite ) );
            System.Diagnostics.Trace.WriteLine( String.Format( "StackTrace: {0}", exception.StackTrace ) );
            //System.Diagnostics.Trace.WriteLine( String.Format( "ToString(): {0}", exception.ToString() ) );
            System.Diagnostics.Trace.WriteLine( String.Format( "Data: {0}", exception.StackTrace ) );
            if( exception.Data != null && exception.Data.Count > 0 )
            {
                System.Diagnostics.Trace.Indent();
                foreach( object key in exception.Data.Keys )
                {
                    System.Diagnostics.Trace.WriteLine( String.Format( "{0} = {1}", key, exception.Data[ key ] ) );
                }
                System.Diagnostics.Trace.Unindent();
            }
            if( exception.InnerException != null )
            {
                System.Diagnostics.Trace.WriteLine( "Inner Exception:" );
                _traceExceptionDetails( exception.InnerException );
            }
            System.Diagnostics.Trace.Unindent();
        }

        // TODO: Add TraceIf versions.  JF
        public static void Trace( object value )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            _traceObject( now, threadInfo, callerInfo, EventClass.Audit, value );
        }

        public static void Trace( EventClass eventClass, object value )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return;
            }
            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            _traceObject( now, threadInfo, callerInfo, eventClass, value );
        }

        private static void _traceObject( string timestamp, string threadInfo, string callerInfo, EventClass eventClass, object value )
        {
            System.Diagnostics.Trace.WriteLine( String.Format( "{0} {1}    {2}  {3} #SPY", timestamp, threadInfo, _getEventClassSymbol( eventClass ), callerInfo ) );
            if( value != null )
            {
                System.Diagnostics.Trace.WriteLine( ">>" );
                System.Diagnostics.Trace.WriteLine( value );
                System.Diagnostics.Trace.WriteLine( "<<" );
            }
        }

        // TODO: Add TraceIf versions.  JF
        public static void TraceBlock()
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            _traceBlock( now, threadInfo, callerInfo, EventClass.Audit, string.Empty );
        }

        public static void TraceBlock( string message )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            _traceBlock( now, threadInfo, callerInfo, EventClass.Audit, message );
        }

        public static void TraceBlock( EventClass eventClass, string message )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            _traceBlock( now, threadInfo, callerInfo, eventClass, message );
        }

        public static void TraceBlock( string message, params object[] args )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            _traceBlock( now, threadInfo, callerInfo, EventClass.Audit, String.Format( message, args ) );
        }

        public static void TraceBlock( EventClass eventClass, string message, params object[] args )
        {
            if( !_haveTraceListeners()
                || !IsEnabled )
            {
                return;
            }

            string now = _getNow();
            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            string threadInfo = _formatThreadInfoText();
            _traceBlock( now, threadInfo, callerInfo, eventClass, String.Format( message, args ) );
        }

        private static void _traceBlock( string timestamp, string threadInfo, string callerInfo, EventClass eventClass, string message )
        {
            System.Diagnostics.Trace.WriteLine( String.Format( "{0} {1}    {2}  {3} #SPY", timestamp, threadInfo, _getEventClassSymbol( eventClass ), callerInfo ) );
            if( message != null && message != string.Empty )
            {
                System.Diagnostics.Trace.WriteLine( ">>" );
                System.Diagnostics.Trace.WriteLine( message );
                System.Diagnostics.Trace.WriteLine( "<<" );
            }
        }

        #region Debug
        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead.") ]
        public static void Debug( Exception exception )
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debug( callerInfo, EventClass.Error, exception );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        private static void _debug( string callerInfo, EventClass eventClass, Exception exception )
        {
            System.Diagnostics.Debug.WriteLine( String.Format( "_________ {0} _________  {1} #DEBUG", eventClass.ToString().ToUpper(), callerInfo ) );
            if( exception != null )
            {
                System.Diagnostics.Debug.WriteLine( ">>" );
                System.Diagnostics.Debug.WriteLine( "Exception:" );
                _debugExceptionDetails( exception );
                System.Diagnostics.Debug.WriteLine( "<<" );
            }
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        private static void _debugExceptionDetails( Exception exception )
        {
            System.Diagnostics.Debug.Indent();
            Type type = exception.GetType();
            System.Diagnostics.Debug.WriteLine( String.Format( "Type: {0} ({1})", type.Name, type.FullName ) );
            System.Diagnostics.Debug.WriteLine( String.Format( "Message: {0}", exception.Message.Replace( Environment.NewLine, @"\r\n" ) ) );
            System.Diagnostics.Debug.WriteLine( String.Format( "Source: {0}", exception.Source ) );
            System.Diagnostics.Debug.WriteLine( String.Format( "TargetSite: {0}", exception.TargetSite ) );
            System.Diagnostics.Debug.WriteLine( String.Format( "StackTrace: {0}", exception.StackTrace ) );
            //System.Diagnostics.Debug.WriteLine( String.Format( "ToString(): {0}", exception.ToString() ) );
            System.Diagnostics.Debug.WriteLine( String.Format( "Data: {0}", exception.StackTrace ) );
            if( exception.Data != null && exception.Data.Count > 0 )
            {
                System.Diagnostics.Debug.Indent();
                foreach( object key in exception.Data.Keys )
                {
                    System.Diagnostics.Debug.WriteLine( String.Format( "{0} = {1}", key, exception.Data[ key ] ) );
                }
                System.Diagnostics.Debug.Unindent();
            }
            if( exception.InnerException != null )
            {
                System.Diagnostics.Debug.WriteLine( "Inner Exception:" );
                _debugExceptionDetails( exception.InnerException );
            }
            System.Diagnostics.Debug.Unindent();
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        public static void Debug( object value )
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debug( callerInfo, EventClass.Audit, value );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        public static void Debug( EventClass eventClass, object value )
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debug( callerInfo, eventClass, value );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        private static void _debug( string callerInfo, EventClass eventClass, object value )
        {
            System.Diagnostics.Debug.WriteLine( String.Format( "_________ {0} _________  {1} #DEBUG", eventClass.ToString().ToUpper(), callerInfo ) );
            if( value != null )
            {
                System.Diagnostics.Debug.WriteLine( ">>" );
                System.Diagnostics.Debug.WriteLine( value );
                System.Diagnostics.Debug.WriteLine( "<<" );
            }
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        public static void Debug()
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debug( callerInfo, EventClass.Audit, string.Empty );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        public static void Debug( string message )
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debug( callerInfo, EventClass.Audit, message );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        public static void Debug( EventClass eventClass, string message )
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debug( callerInfo, eventClass, message );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        public static void Debug( string message, params object[] args )
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debug( callerInfo, EventClass.Audit, String.Format( message, args ) );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        public static void Debug( EventClass eventClass, string message, params object[] args )
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debug( callerInfo, eventClass, String.Format( message, args ) );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        private static void _debug( string callerInfo, EventClass eventClass, string message )
        {
            System.Diagnostics.Debug.WriteLine( String.Format( "_________ {0} _________  {1} #DEBUG", eventClass.ToString().ToUpper(), callerInfo ) );
            if( message != null && message != string.Empty )
            {
                System.Diagnostics.Debug.WriteLine( ">>" );
                System.Diagnostics.Debug.WriteLine( message );
                System.Diagnostics.Debug.WriteLine( "<<" );
            }
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        public static void DebugLine( string message )
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debugLine( callerInfo, EventClass.Audit, message );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        public static void DebugLine( EventClass eventClass, string message )
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debugLine( callerInfo, eventClass, message );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        public static void DebugLine( string message, params object[] args )
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debugLine( callerInfo, EventClass.Audit, String.Format( message, args ) );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        public static void DebugLine( EventClass eventClass, string message, params object[] args )
        {
            if( !_haveDebugListeners() )
            {
                return;
            }

            StackFrame callerFrame = _getCallerStackFrame();
            string callerInfo = _formatCallerInfoText( callerFrame );
            _debugLine( callerInfo, eventClass, String.Format( message, args ) );
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        private static void _debugLine( string callerInfo, EventClass eventClass, string message )
        {
            System.Diagnostics.Trace.WriteLine( String.Format( "{0} {1}  {2} #DEBUG", _getEventClassSymbol( eventClass ), message.Replace( Environment.NewLine, @"\r\n" ), callerInfo ) );
        }
        #endregion Debug


        // NOTE: VERY IMPORTANT! Because the stack frame depth is hard-coded to 2 this must always be directly and only called by public
        //  methods that are being called from the constituent method.  Otherwise it's going to look like a different method printed the
        //  message.  JF
        private static StackFrame _getCallerStackFrame()
        {
            StackTrace stackTrace = new StackTrace( 2, true );
            return stackTrace.GetFrame( 0 );
        }

        private static string _formatCallerInfoText( StackFrame callerFrame )
        {
            return String.Format( "{0}.{1}():line {2}", callerFrame.GetMethod().DeclaringType.FullName, callerFrame.GetMethod().Name, callerFrame.GetFileLineNumber().ToString() );
            //string caller = String.Format( "{0}.{1}():line {2}", callerFrame.GetMethod().DeclaringType.FullName, callerFrame.GetMethod().Name, callerFrame.GetFileLineNumber().ToString() );
            //string info = String.Format( "[{0}] (ThreadId:{1})", caller, Thread.CurrentThread.ManagedThreadId.ToString() );
            //return info;
        }

        private static string _formatThreadInfoText()
        {
            return String.Format( "TID:{0}", Thread.CurrentThread.ManagedThreadId.ToString( CultureInfo.InvariantCulture ).PadLeft( 5, '0' ) );
        }

        private static bool _haveTraceListeners()
        {
            if( System.Diagnostics.Trace.Listeners == null || System.Diagnostics.Trace.Listeners.Count < 1 )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [Obsolete( "All .Debug() methods on Spy are deprecated.  Use the corresponding .Trace() methods instead." )]
        private static bool _haveDebugListeners()
        {
            if( System.Diagnostics.Debug.Listeners == null || System.Diagnostics.Debug.Listeners.Count < 1 )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static string _getEventClassSymbol( EventClass eventClass )
        {
            if( eventClass == EventClass.Audit )
            {
                return "-";
            }
            else if( eventClass == EventClass.Warning )
            {
                return "?";
            }
            else
            {
                return "!";
            }
        }

        private static string _getNow()
        {
            // TODO: Or UtcNow? JF
            return DateTime.Now.ToString( "yyyy'-'MM'-'dd' 'HH':'mm':'ss" );
        }
    }

    // TODO: Refactor into a factype with the "Symbol" from _getEventClassSymbol() as a property.  JF
    public enum EventClass
    {
        Audit,
        Warning,
        Error
    }
}
