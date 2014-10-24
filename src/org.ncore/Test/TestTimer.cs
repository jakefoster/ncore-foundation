using System;
using System.Diagnostics;
using org.ncore.Diagnostics;
using org.ncore.Extensions;

namespace org.ncore.Test
{
    public class TestTimer : IDisposable
    {
        private static int _depth = -1;

        private DateTime _startTime;
        private StackFrame _stackFrame;
        private string _methodName;
        private string _label;

        public static TestTimer New()
        {
            return TestTimer._new( string.Empty );
        }

        public static TestTimer New( string label )
        {
            return TestTimer._new( label );
        }

        private static TestTimer _new( string label )
        {
            TestTimer testTimer = new TestTimer( label );
            testTimer.start( 3 );
            return testTimer;
        }

        private TestTimer( string label )
        {
            _label = label;
            TestTimer._depth++;
        }

        public void Start()
        {
            start( 2 );
        }

        internal void start( int stackFrameOffset )
        {
            _stackFrame = new System.Diagnostics.StackFrame( stackFrameOffset, true );
            _methodName = _stackFrame.GetMethod().DeclaringType.ToString() + "::" + _stackFrame.GetMethod().Name + "()";            
            _startTime = DateTime.Now;
            if( _label == null || _label == string.Empty )
            {
                _label = "Line " + _stackFrame.GetFileLineNumber().ToString();
            }

            Spy.Trace( indent() + "({0}) Starting timer '{1}' in {2}", (_depth + 1).ToString(), _label, _methodName );
        }

        private string indent()
        {
            return string.Empty.Fill( " ", TestTimer._depth * 4 );
        }

        public void Dispose()
        {
            DateTime endTime = DateTime.Now;
            TimeSpan elapsed = new TimeSpan( endTime.Ticks - _startTime.Ticks );
            Spy.Trace( indent() + "({0}) Stopped timer '{1}' [{2} milliseconds] in {3}", ( _depth + 1 ).ToString(), _label, elapsed.TotalMilliseconds, _methodName );            
            TestTimer._depth--;
        }
    }
}
