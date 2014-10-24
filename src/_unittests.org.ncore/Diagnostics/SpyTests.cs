using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Common;
using org.ncore.Diagnostics;
using _unittests.org.ncore;
using org.ncore.Extensions;
using org.ncore.Test;

namespace _unittests.org.ncore.Diagnostics
{
    // TODO: Need to heavily refactor these tests to be less expository.  There are lots of 
    //  really big demo "tests" that have no assertions and aren't really tests at all, and
    //  basically no good, fine-grained verifications of actually functionality.  JF

    /// <summary>
    /// Summary description for SpyTests
    /// </summary>
    [TestClass]
    public class SpyTests
    {
        private UnitTestTraceListener _listener;
        private StringBuilder _trace;

        public SpyTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }

        //Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            _trace = new StringBuilder();
            _listener = new UnitTestTraceListener( _trace );
            Trace.Listeners.Add( _listener );
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Trace.Listeners.Remove( _listener );
            _listener.Dispose();
            _listener = null;
            _trace = null;
        }

        #endregion

        [TestMethod]
        public void TrapTraceMark_Demo_ToFile()
        {
            TextFileWriter writer = new TextFileWriter();
            writer.FileLocation = Path.Combine( Environment.CurrentDirectory, "TextFileTraceListener.txt" );

            TextFileTraceListener listner = new TextFileTraceListener( writer );
            try
            {
                Trace.Listeners.Add( listner );
                List<string> foos = new List<string>() {"zero", "one", "two", "three"};
                var domain = "matrix";
                int amount = 123;

                Parent parent = new Parent() {Identity = "MyParent"};
                Child child1 = new Child( parent ) {Identity = "MyParent's first child"};
                Child child2 = new Child( parent ) {Identity = "MyParent's second child"};

                Stopwatch stopwatch = Stopwatch.StartNew();
                for( int i = 0; i < 5; i++ )
                {
                    Spy.Mark();
                    Spy.Trap( () => i );
                    Spy.TraceBlock( "Starting\r\nup!" );
                    Spy.Trap( () => this );
                    Spy.Trap( () => amount );
                    Spy.Mark();
                    Spy.Trace( "Starting up!" );
                    Spy.Trace( EventClass.Error, "foo {0}", "bar" );
                    Spy.Trace( EventClass.Warning, "snafu{0}", 1 );
                    Spy.Trace( EventClass.Audit, "whatever" );
                    ArgumentException ex = new ArgumentException( "Foobarred!", "MyParam" );
                    Spy.Trace( ex );
                    ApplicationException ex2 = new ApplicationException( "Something bad happened", ex );
                    Spy.Trace( ex2 );
                    Spy.Trace( EventClass.Audit, "whatever" );
                    Spy.Trap( () => domain.Length );
                    Spy.Trace( EventClass.Audit, "whatever" );
                    Spy.Trace( parent );
                    Spy.Trace( EventClass.Audit, "whatever" );
                    Spy.Trap( () => parent.Children[ 1 ].Identity );
                    Spy.Trap( () => child1.Parent.Identity );
                    Spy.Trap( () => child1.Parent );
                    Spy.Trace( EventClass.Audit, "whatever" );
                    Spy.Trap( () => foos[ 1 ].Length );
                    Spy.Trace( EventClass.Audit, "whatever" );
                    Spy.Trap( () => domain );
                    Spy.TraceBlock( "Starting\r\ndown!" );
                    Spy.Trace( EventClass.Audit, "whatever" + Environment.NewLine + "pooh!" );
                    Spy.Trap( () => foos.Where( foo => foo.Length > 3 ).Count() );
                    Spy.Trace( EventClass.Audit, "whatever" );
                    Spy.Trap( () => domain == "matrix" && foos.Count == 1 || ( foos.Contains( "two" ) ) );
                    Spy.Trace( EventClass.Audit, "whatever and {0} and {1}.", "ever", "never" );
                    Spy.Trace( "Leaving\r\nthis\r\nmethod\r\nnow." );
                    Spy.Trap( () => stopwatch.ElapsedMilliseconds );
                }
                stopwatch.Stop();

                Spy.Trace( stopwatch.ElapsedMilliseconds );

                // TODO: Uhh.  Shouldn't we like, open the file and look at it or something?  JF
                Assert.IsTrue( _trace.Length > 0 );
                Assert.IsTrue( _trace.ToString().Contains( "Starting up!" ) );
            }
            finally
            {
                Trace.Listeners.Remove( listner );
            }
        }

        [TestMethod]
        public void TrapTraceMark_Demo()
        {
            List<string> foos = new List<string>() { "zero", "one", "two", "three" };
            var domain = "matrix";
            int amount = 123;

            Parent parent = new Parent() { Identity = "MyParent" };
            Child child1 = new Child( parent ) { Identity = "MyParent's first child" };
            Child child2 = new Child( parent ) { Identity = "MyParent's second child" };

            Stopwatch stopwatch = Stopwatch.StartNew();
            for( int i = 0; i < 10; i++ )
            {
                Spy.Mark();
                Spy.Trap( () => i );
                Spy.TraceBlock( "Starting\r\nup!" );
                Spy.Trap( () => this );
                Spy.Trap( () => amount );
                Spy.Mark();
                Spy.Trace( "Starting up!" );
                Spy.Trace( EventClass.Error, "foo {0}", "bar" );
                Spy.Trace( EventClass.Warning, "snafu{0}", 1 );
                Spy.Trace( EventClass.Audit, "whatever" );
                ArgumentException ex = new ArgumentException( "Foobarred!", "MyParam" );
                Spy.Trace( ex );
                ApplicationException ex2 = new ApplicationException( "Something bad happened", ex );
                Spy.Trace( ex2 );
                Spy.Trace( EventClass.Audit, "whatever" );
                Spy.Trap( () => domain.Length );
                Spy.Trace( EventClass.Audit, "whatever" );
                Spy.Trace( parent );
                Spy.Trace( EventClass.Audit, "whatever" );
                Spy.Trap( () => parent.Children[ 1 ].Identity );
                Spy.Trap( () => child1.Parent.Identity );
                Spy.Trap( () => child1.Parent );
                Spy.Trace( EventClass.Audit, "whatever" );
                Spy.Trap( () => foos[ 1 ].Length );
                Spy.Trace( EventClass.Audit, "whatever" );
                Spy.Trap( () => domain );
                Spy.TraceBlock( "Starting\r\ndown!" );
                Spy.Trace( EventClass.Audit, "whatever" + Environment.NewLine + "pooh!" );
                Spy.Trap( () => foos.Where( foo => foo.Length > 3 ).Count() );
                Spy.Trace( EventClass.Audit, "whatever" );
                Spy.Trap( () => domain == "matrix" && foos.Count == 1 || ( foos.Contains( "two" ) ) );
                Spy.Trace( EventClass.Audit, "whatever and {0} and {1}.", "ever", "never" );
                Spy.Trace( "Leaving\r\nthis\r\nmethod\r\nnow." );
                Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            }
            stopwatch.Stop();

            Spy.Trace( stopwatch.ElapsedMilliseconds );

            Assert.IsTrue( _trace.Length > 0 );
            Assert.IsTrue( _trace.ToString().Contains( "Starting up!" ) );
        }

        [TestMethod]
        public void Performance()
        {
            List<string> foos = new List<string>() { "zero", "one", "two", "three" };
            var domain = "matrix";
            int amount = 123;

            Parent parent = new Parent() { Identity = "MyParent" };
            Child child1 = new Child( parent ) { Identity = "MyParent's first child" };
            Child child2 = new Child( parent ) { Identity = "MyParent's second child" };

            Stopwatch stopwatch = Stopwatch.StartNew();

            Spy.Mark();
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.TraceBlock( "Starting\r\nup!" );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trap( () => this );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trap( () => amount );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( "Starting up!" );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( EventClass.Error, "foo {0}", "bar" );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( EventClass.Warning, "snafu{0}", 1 );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( EventClass.Audit, "whatever" );
            ArgumentException ex = new ArgumentException( "Foobarred!", "MyParam" );
            Spy.Trace( ex );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            ApplicationException ex2 = new ApplicationException( "Something bad happened", ex );
            Spy.Trace( ex2 );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( EventClass.Audit, "whatever" );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trap( () => domain.Length );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( EventClass.Audit, "whatever" );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( parent );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( EventClass.Audit, "whatever" );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trap( () => parent.Children[ 1 ].Identity );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trap( () => child1.Parent.Identity );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trap( () => child1.Parent );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( EventClass.Audit, "whatever" );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trap( () => foos[ 1 ].Length );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trap( () => domain );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.TraceBlock( "Starting\r\ndown!" );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( EventClass.Audit, "whatever" + Environment.NewLine + "pooh!" );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trap( () => foos.Where( foo => foo.Length > 3 ).Count() );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( EventClass.Audit, "whatever" );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trap( () => domain == "matrix" && foos.Count == 1 || ( foos.Contains( "two" ) ) );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( EventClass.Audit, "whatever and {0} and {1}.", "ever", "never" );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );
            Spy.Trace( "Leaving\r\nthis\r\nmethod\r\nnow." );
            Spy.Trap( () => stopwatch.ElapsedMilliseconds );

            stopwatch.Stop();

            Spy.Trace( stopwatch.ElapsedMilliseconds );

            Assert.IsTrue( _trace.Length > 0 );
            Assert.IsTrue( _trace.ToString().Contains( "Starting up!" ) );
        }

        [TestMethod]
        public void IsEnabled_DefaultTrue()
        {
            // ARRANGE, ACT & ASSERT
            Assert.AreEqual( true, Spy.IsEnabled );
        }

        [TestMethod]
        public void IsEnabled_ExplicitTrue()
        {
            // ARRANGE
            _removeAppSettings();
            _createAppSetting( "org.ncore.Diagnostics/Spy.IsEnabled", "true" );

            // ACT & ASSERT  :)
            Assert.AreEqual( true, Spy.IsEnabled );
            _removeAppSettings();
        }

        [TestMethod]
        public void IsEnabled_ExplicitFalse()
        {
            // ARRANGE
            _removeAppSettings();
            _createAppSetting( "org.ncore.Diagnostics/Spy.IsEnabled", "false" );

            // ACT & ASSERT  :)
            Assert.AreEqual( false, Spy.IsEnabled );
            _removeAppSettings();
        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void IsEnabled_JunkConfigurationData()
        {
            // ARRANGE
            _removeAppSettings();
            _createAppSetting( "org.ncore.Diagnostics/Spy.IsEnabled", "notvalid" );

            // ACT & ASSERT  :)
            try
            {
                Assert.AreEqual( true, Spy.IsEnabled );
            }
            finally
            {
                _removeAppSettings();
            }
        }

        // TODO: More tests around IsEnabled?  JF
        [TestMethod]
        public void Mark_IsEnabled_False()
        {
            // ARRANGE
            _removeAppSettings();
            _createAppSetting( "org.ncore.Diagnostics/Spy.IsEnabled", "false" );

            // ACT
            Spy.Mark();

            // ASSERT
            Assert.IsTrue( _trace.Length == 0 );

            _removeAppSettings();
        }

        [TestMethod]
        public void Trap_IsEnabled_False()
        {
            // ARRANGE
            _removeAppSettings();
            _createAppSetting( "org.ncore.Diagnostics/Spy.IsEnabled", "false" );

            // ACT
            Spy.Trap( () => this );

            // ASSERT
            Assert.IsTrue( _trace.Length == 0 );

            _removeAppSettings();
        }

        [TestMethod]
        public void Mark()
        {
            // ARRANGE

            // ACT
            Spy.Mark();

            // ASSERT
            Assert.IsTrue( _trace.Length > 0 );
            // TODO: WOuld be better with a regex here.  JF
            Assert.IsTrue( _trace.ToString().Contains( "_________ SpyTests.Mark() _________  [_unittests.org.ncore.Diagnostics.SpyTests.Mark():line " ) );
            Assert.IsTrue( _trace.ToString().EndsWith( " #SPY" ) );
        }

        [TestMethod]
        public void MarkWithLineNumber()
        {
            // ARRANGE

            // ACT
            Spy.Mark( true );

            // ASSERT
            Assert.IsTrue( _trace.Length > 0 );
            // TODO: WOuld be better with a regex here.  JF
            Assert.IsTrue( _trace.ToString().Contains( "TID:" ) );
            Assert.IsTrue( _trace.ToString().Contains( "_________ SpyTests.MarkWithLineNumber()  @@" ) );
            Assert.IsTrue( _trace.ToString().Contains( " _________  [_unittests.org.ncore.Diagnostics.SpyTests.MarkWithLineNumber():line " ) );
            Assert.IsTrue( _trace.ToString().EndsWith( " #SPY" ) );
        }

        [TestMethod]
        public void MarkWithLabel()
        {
            // ARRANGE
            string labelText = "Testing .Mark() method (with label)";
            // ACT
            Spy.Mark( labelText );

            // ASSERT
            Assert.IsTrue( _trace.Length > 0 );
            // TODO: WOuld be better with a regex here.  JF
            Assert.IsTrue( _trace.ToString().Contains( "TID:" ) );
            Assert.IsTrue( _trace.ToString().Contains( "_________ SpyTests.MarkWithLabel()  @``" + labelText + "`` _________  [_unittests.org.ncore.Diagnostics.SpyTests.MarkWithLabel():line " ) );
            Assert.IsTrue( _trace.ToString().EndsWith( " #SPY" ) );
        }

        [TestMethod]
        public void MarkWithLabelAndLineNumber()
        {
            // ARRANGE
            string labelText = "Testing .Mark() method (with label)";
            // ACT
            Spy.Mark( labelText, true );

            // ASSERT
            Assert.IsTrue( _trace.Length > 0 );
            // TODO: WOuld be better with a regex here.  JF
            Assert.IsTrue( _trace.ToString().Contains( "TID:" ) );
            Assert.IsTrue( _trace.ToString().Contains( "_________ SpyTests.MarkWithLabelAndLineNumber()  @``" + labelText + "``@@" ) );
            Assert.IsTrue( _trace.ToString().Contains( " _________  [_unittests.org.ncore.Diagnostics.SpyTests.MarkWithLabelAndLineNumber():line " ) );
            Assert.IsTrue( _trace.ToString().EndsWith( " #SPY" ) );
        }

        [TestMethod]
        public void Trap()
        {
            // ARRANGE

            // ACT
            Spy.Trap( () => this );

            List<string> foos = new List<string>() { "zero", "one", "two", "three" };
            var text = "mytext";
            int amount = 123;
            Spy.Trap( () => text );
            Spy.Trap( () => text.Length );
            Spy.Trap( () => amount );
            Spy.Trap( () => foos[ 1 ].Length );
            Spy.Trap( () => foos.Where( foo => foo.Length > 3 ).Count() );
            Spy.Trap( () => text == "mytext" && foos.Count == 1 || ( foos.Contains( "two" ) ) );

            Parent parent = new Parent() { Identity = "MyParent" };
            Child child1 = new Child( parent ) { Identity = "MyParent's first child" };
            Child child2 = new Child( parent ) { Identity = "MyParent's second child" };

            Spy.Trap( () => parent.Children[ 1 ].Identity );

            Spy.Trap( () => child1.Parent.Identity );

            Spy.Trap( () => child1.Parent );

            Spy.Trap( () => child2 );

            // ASSERT
            // TODO: Need some real assertions here.  JF
            Assert.IsTrue( _trace.Length > 0 );
        }

        [TestMethod]
        public void Trap_StaticProperty()
        {
            // ARRANGE

            // ACT
            Spy.Trap( () => Parent.MySetting );

            // ASSERT
            Assert.IsTrue( _trace.Length > 0 );
        }

        [TestMethod]
        public void Trap_MemberVariable()
        {
            // ARRANGE

            // ACT
            Spy.Trap( () => _trace );

            // ASSERT
            Assert.IsTrue( _trace.Length > 0 );
        }

        [TestMethod]
        public void TraceException()
        {
            ArgumentException ex = new ArgumentException( "Foobarred!", "MyParam" );
            Spy.Trace( ex );

            ApplicationException ex2 = new ApplicationException( "Something bad happened", ex );
            Spy.Trace( ex2 );

            Assert.IsTrue( _trace.Length > 0 );
            // TODO: WOuld be better with a regex here.  JF
            //Assert.IsTrue( trace.ToString().Contains( "_________ MARK _________  [_unittests.org.ncore.Diagnostics.SpyTests.Mark():line " ) );
            //Assert.IsTrue( trace.ToString().Contains( "] (ThreadId:" ) );
            //Assert.IsTrue( trace.ToString().Contains( ") #SPY" ) );
        }

        [TestMethod]
        public void TraceObject()
        {
            Parent parent = new Parent() { Identity = "MyParent" };
            Child child1 = new Child( parent ) { Identity = "MyParent's first child" };
            Child child2 = new Child( parent ) { Identity = "MyParent's second child" };

            Spy.Trace( parent );

            Assert.IsTrue( _trace.Length > 0 );
            // TODO: WOuld be better with a regex here.  JF
            //Assert.IsTrue( trace.ToString().Contains( "_________ MARK _________  [_unittests.org.ncore.Diagnostics.SpyTests.Mark():line " ) );
            //Assert.IsTrue( trace.ToString().Contains( "] (ThreadId:" ) );
            //Assert.IsTrue( trace.ToString().Contains( ") #SPY" ) );
        }


        [TestMethod]
        public void TraceBlock()
        {
            Spy.TraceBlock( "Starting\r\nup!" );

            Assert.IsTrue( _trace.Length > 0 );
            Assert.IsTrue( _trace.ToString().Contains( "Starting\r\nup!" ) );
        }

        [TestMethod]
        public void Trace_Simple()
        {
            Spy.Mark();
            Spy.Trace( "Starting up!" );
            Spy.Trace( EventClass.Error, "foo {0}", "bar" );
            Spy.Trace( EventClass.Warning, "snafu{0}", 1 );
            Spy.Trace( EventClass.Audit, "whatever" );
            Spy.Trace( EventClass.Audit, "whatever" );
            Spy.Trace( EventClass.Audit, "whatever" );

            ArgumentException ex = new ArgumentException( "Foobarred!", "MyParam" );
            Spy.Trace( ex );

            ApplicationException ex2 = new ApplicationException( "Something bad happened", ex );
            Spy.Trace( ex2 );


            Spy.Trace( EventClass.Audit, "whatever" );
            Spy.Trace( EventClass.Audit, "whatever" );
            Spy.Trace( EventClass.Audit, "whatever" );
            Spy.Trace( EventClass.Audit, "whatever" + Environment.NewLine + "pooh!" );
            Spy.Trace( EventClass.Audit, "whatever" );
            Spy.Trace( EventClass.Audit, "whatever and {0} and {1}.", "ever", "never" );
            Spy.Trace( "Leaving\r\nthis\r\nmethod\r\nnow." );

            Assert.IsTrue( _trace.Length > 0 );
            Assert.IsTrue( _trace.ToString().Contains( "Starting up!" ) );
        }

        // TODO: Need real unit tests.  JF

        [TestMethod]
        public void Trap_Null()
        {
            string text = null;
            Spy.Trap( () => text );

            Assert.IsTrue( _trace.Length > 0 );
        }

        [TestMethod]
        public void TrapIf_False()
        {
            Spy.TrapIf( false == true, () => this );

            Assert.IsTrue( _trace.Length == 0 );
            Assert.AreEqual( string.Empty, _trace.ToString() );
        }

        [TestMethod]
        public void TrapIf_True()
        {
            Spy.TrapIf( true == true, () => this );

            Assert.IsTrue( _trace.Length > 0 );
            Assert.IsTrue( _trace.ToString().Contains( "this" ) );
        }

        [TestMethod]
        public void TraceIf_False()
        {
            Spy.TraceIf( false == true, "traced" );

            Assert.IsTrue( _trace.Length == 0 );
            Assert.AreEqual( string.Empty, _trace.ToString() );
        }

        [TestMethod]
        public void TraceIf_True()
        {
            Spy.TraceIf( true == true, "traced" );

            Assert.IsTrue( _trace.Length > 0 );
            Assert.IsTrue( _trace.ToString().Contains( "traced" ) );
        }
        
        public void _createAppSetting( string key, string value )
        {
            Configuration appConfiguration = ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None );
            AppSettingsSection configuration = null;
            if( appConfiguration.Sections[ "appSettings" ] == null )
            {
                configuration = new AppSettingsSection();
                appConfiguration.Sections.Add( "appSettings", configuration );
            }
            else
            {
                configuration = (AppSettingsSection)appConfiguration.GetSection( "appSettings" );
            }

            if( configuration.Settings[ key ] != null )
            {
                configuration.Settings.Remove( key );
            }

            KeyValueConfigurationElement setting = new KeyValueConfigurationElement( key, value );
            configuration.Settings.Add( setting );

            appConfiguration.Save( ConfigurationSaveMode.Modified );
            ConfigurationManager.RefreshSection( "appSettings" );
        }

        // NOTE: Not thread safe!  JF
        private void _removeAppSettings()
        {
            Configuration appConfiguration = ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None );

            if( appConfiguration.Sections[ "appSettings" ] != null )
            {
                appConfiguration.Sections.Remove( "appSettings" );
            }

            appConfiguration.Save( ConfigurationSaveMode.Modified );
            ConfigurationManager.RefreshSection( "appSettings" );
        }
    }

    internal class Parent
    {
        public static string MySetting
        {
            get { return "MySettingValue"; }
        }

        public string Identity { get; set; }
        public List<Child> Children { get; private set; }

        public Parent()
        {
            Children = new List<Child>();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine( this.Identity );
            builder.AppendLine( "Children" );
            builder.AppendLine( "{" );
            foreach( Child child in this.Children )
            {
                builder.AppendLine( "\t" + child.Identity );
            }
            builder.AppendLine( "}" );
            return builder.ToString();
        }
    }

    internal class Child
    {
        public string Identity { get; set; }
        public Parent Parent { get; private set; }

        public Child( Parent parent )
        {
            this.Parent = parent;
            this.Parent.Children.Add( this );
        }
    }

    public class TextFileTraceListener : TraceListener
    {
        private TextFileWriter _target;

        public TextFileTraceListener( TextFileWriter target )
        {
            _target = target;
        }

        public override void Write( string message )
        {
            this.WriteIndent();
            _target.Write( message );
        }

        protected override void WriteIndent()
        {
            if( this.IndentLevel > 0 )
            {
                _target.Write( string.Empty.Fill( " ", ( this.IndentLevel * this.IndentSize ) ) );
            }
        }

        public override void WriteLine( string message )
        {
            this.WriteIndent();
            _target.WriteLine( message );
        }
    }
}
