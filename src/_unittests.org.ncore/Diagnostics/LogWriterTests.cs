using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Common;
using org.ncore.Diagnostics;
using org.ncore.Extensions;

namespace _unittests.org.ncore.Diagnostics
{
    /// <summary>
    /// Summary description for LogWriterTests
    /// </summary>
    [TestClass]
    public class LogWriterTests
    {
        // TODO: There are definitely some unit tests missing from this TestFixture.  For the sake of expediency we have NOT
        //	implemented all possible unit tests for the LogWriter class.  For example, not all overloads of the Write() method
        //	are exercised.  Additionally, most tests focus on the file logging output and do not examine the EventLog or
        //	other Console targets.  These alternative targets are tested only superficially to ensure that basic functionality
        //	for their respective logging is working.  Obviously this issue should be addressed as soon as possible to ensure
        //	full unit test coverage for this assembly.
        // TODO: Need to write unit tests to ensure thread saftey and proper concurrency behavior.  I.e. writers on multiple threads writing to the same log at the same time.  See Write_MultiThreaded() unit test below (currently commented out.)
        // TODO: Should have a unit test that ensures proper behavior when writing to multiple LogWriter instances from the same caller.
        
        private static string[] _parseLineFromFile( string fileName, int targetLineNumber = 1 )
        {
            if( File.Exists( fileName ) )
            {
                FileStream fileStream = File.Open( fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read,
                                                   System.IO.FileShare.ReadWrite );
                string targetLine = _readLineFromStream( fileStream, targetLineNumber );
                
                fileStream.Close();

                return targetLine.Split( '\x1F' );
            }
            else
            {
                throw new ApplicationException( "Could not read file because the file does not exist.  File name: " + fileName );
            }
        }

        private static string _readLineFromStream( Stream stream, int targetLineNumber )
        {
            Condition.Assert( targetLineNumber > 0, new ArgumentException( "Value must be greater than 0.", "targetLineNumber" ) );
            
            StreamReader streamReader = new StreamReader( stream );
            streamReader.BaseStream.Seek( 0, SeekOrigin.Begin );

            string targetLine = string.Empty;
            string currentLine;
            int currentLineNumber = 0;
            do
            {
                currentLineNumber++;
                currentLine = streamReader.ReadLine();
                if( currentLineNumber == targetLineNumber )
                {
                    targetLine = currentLine;
                    break;
                }
            } while( currentLine != null );

            return targetLine;
        }

        private static void _deleteFile( string fileName )
        {
            if( File.Exists( fileName ) )
            {
                File.Delete( fileName );
            }
        }

        private LogWriter _log;

        private static string _eventLogSource = "NCoreUnitTests";

        public LogWriterTests()
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
        // TODO: Need to deal with changes (as of Vista) that prevent an event log source being created on the fly and used right away.
        //  See below comments in sample at: http://msdn.microsoft.com/en-us/library/system.diagnostics.eventlog.aspx  JF
        //      "An event log source should not be created and immediately used.
        //       There is a latency time to enable the source, it should be created
        //       prior to executing the application that uses the source.
        //       Execute this sample a second time to use the new source."
        //  UPDATE: Seems to have been addressed by putting source creation here in TestFixture setup/teardown.  Verify?  JF
        // TODO: Also need to deal with the issue when attempting to create the new source:
        //      "The source was not found, but some or all event logs could not be searched.  Inaccessible logs: Security."
        //  for more info see http://social.msdn.microsoft.com/Forums/en-US/windowsgeneraldevelopmentissues/thread/416098a4-4183-4711-a53b-e10966c9801d/
        //  and here's the (concise) permissions fix in the registry: http://support.microsoft.com/kb/842795
        //  Simple enough, but a *very* manual user action (as is setting up the EventLog source that is to be written to)
        //  This isn't really something we can do in a unit test setup so the question is how we deal with this 
        //  so that tests will just run all green when someone runs them for the first time?  JF
        [ClassInitialize()]
        public static void MyClassInitialize( TestContext testContext )
        {
            try
            {
                // NOTE: This is an auxilliary step that needs to happen just once to create 
                //  the EventLog source on the machine, then never again.  Unfortunately, 
                //  without manually setting permissions on the event log hive in the registry 
                //  for the executing user this will fail (as will all the event logging unit
                //  tests.)  Most unfortunate.  JF
                if( !EventLog.SourceExists( _eventLogSource ) )
                {
                    EventLog.CreateEventSource( _eventLogSource, "Application" );
                }
            }
            catch
            {
                // NOTE: See above comments for explanation of issues with EventLog testing.  
                //  Empty try-catch is a neccessary evil here.  JF
            }
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            // NOTE: See comments in MyClassInitialize() for explanation of issues around EventLog testing.  JF
            try
            {
                if( !EventLog.SourceExists( _eventLogSource ) )
                {
                    EventLog.DeleteEventSource( _eventLogSource, "Application" );
                }
            }
            catch
            {
                // NOTE: See above comments for explanation of issues with EventLog testing.  
                //  Empty try-catch is a neccessary evil here.  JF
            }
        }
        
        [TestInitialize()]
        public void MyTestInitialize()
        {
            _log = new LogWriter();
            _deleteFile( _log.FileLocation );
        }

        [TestCleanup()]
        public void MyTestCleanup() 
        {
            if( _log != null )
            {
                _log.Dispose();
                _log = null;
            }
        }
        #endregion
        
        [TestMethod]
        public void Write()
        {
            using( _log )
            {
                _log.Write( "First entry" );
                _log.Write( "Second entry" );
                _log.Write( "Third (and last) entry" );
            }

            string[] fields = LogWriterTests._parseLineFromFile( _log.FileLocation, 1 );
            DateTime.Parse( fields[ 0 ] ); // Test the date/time is at least valid (it's tough to actually check the date/time value)
            Assert.AreEqual( fields[ 1 ], "First entry" );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write()" );
            Assert.AreEqual( fields[ 3 ], "Information" );
            Assert.AreEqual( fields[ 4 ], "0" );

            fields = LogWriterTests._parseLineFromFile( _log.FileLocation, 2 );
            DateTime.Parse( fields[ 0 ] ); // Test the date/time is at least valid (it's tough to actually check the date/time value)
            Assert.AreEqual( fields[ 1 ], "Second entry" );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write()" );
            Assert.AreEqual( fields[ 3 ], "Information" );
            Assert.AreEqual( fields[ 4 ], "0" );

            fields = LogWriterTests._parseLineFromFile( _log.FileLocation, 3 );
            DateTime.Parse( fields[ 0 ] ); // Test the date/time is at least valid (it's tough to actually check the date/time value)
            Assert.AreEqual( fields[ 1 ], "Third (and last) entry" );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write()" );
            Assert.AreEqual( fields[ 3 ], "Information" );
            Assert.AreEqual( fields[ 4 ], "0" );

            // NOTE: Normally, if you're just writing a single line to the log you can do this:
            //	__log.Write( ...
            // Instead of embedding in the 'using' block.  Just remember to do this:
            //	__log.Dipsose();
            //	__log = null;
            // If you do use it outside of a 'using' block.  Of course, you probably just want to do this once when your app shuts down.
            //  Regardless, the LogWriter will handle it correctly.
        }

        [TestMethod]
        public void Write_WithAdditionalEntryData()
        {
            _log.Write( "Write_WithAdditionalEntryData", "A=1", "B=2", "C=3" );

            string[] fields = LogWriterTests._parseLineFromFile( _log.FileLocation );

            DateTime.Parse( fields[ 0 ] ); // NOTE: Test the date/time is at least valid (it's tough to actually check the date/time value)
            Assert.AreEqual( fields[ 1 ], "Write_WithAdditionalEntryData" );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write_WithAdditionalEntryData()" );
            Assert.AreEqual( fields[ 3 ], "Information" );
            Assert.AreEqual( fields[ 4 ], "0" );
            Assert.AreEqual( fields[ 5 ], "A=1" );
            Assert.AreEqual( fields[ 6 ], "B=2" );
            Assert.AreEqual( fields[ 7 ], "C=3" );
        }
        
        [TestMethod]
        public void Write_WithEntryType()
        {
            _log.Write( "Write_WithEntryType", LogWriter.EntryTypeEnum.Warning );

            string[] fields = LogWriterTests._parseLineFromFile( _log.FileLocation );

            DateTime.Parse( fields[ 0 ] ); // NOTE: Test the date/time is at least valid (it's tough to actually check the date/time value)
            Assert.AreEqual( fields[ 1 ], "Write_WithEntryType" );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write_WithEntryType()" );
            Assert.AreEqual( fields[ 3 ], "Warning" );
            Assert.AreEqual( fields[ 4 ], "0" );
        }

        [TestMethod]
        public void Write_WithEntryTypeAndEventNumber()
        {
            _log.Write( "Write_WithEntryTypeAndEventNumber", LogWriter.EntryTypeEnum.Warning, 123 );

            string[] fields = LogWriterTests._parseLineFromFile( _log.FileLocation );

            DateTime.Parse( fields[ 0 ] ); // NOTE: Test the date/time is at least valid (it's tough to actually check the date/time value)
            Assert.AreEqual( fields[ 1 ], "Write_WithEntryTypeAndEventNumber" );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write_WithEntryTypeAndEventNumber()" );
            Assert.AreEqual( fields[ 3 ], "Warning" );
            Assert.AreEqual( fields[ 4 ], "123" );
        }
        
        [TestMethod]
        public void Write_WithSource()
        {
            _log.Write( "Write_WithSource", "Custom Source", LogWriter.EntryTypeEnum.Information );

            string[] fields = LogWriterTests._parseLineFromFile( _log.FileLocation );

            DateTime.Parse( fields[ 0 ] ); // NOTE: Test the date/time is at least valid (it's tough to actually check the date/time value)
            Assert.AreEqual( fields[ 1 ], "Write_WithSource" );
            Assert.AreEqual( fields[ 2 ], "Custom Source" );
            Assert.AreEqual( fields[ 3 ], "Information" );
            Assert.AreEqual( fields[ 4 ], "0" );
        }

        [TestMethod]
        public void Write_WithException()
        {
            _log.LogTarget = _log.LogTarget | LogWriter.LogTargetEnum.EventLog;
            try
            {
                throw new ApplicationException( "An exception was thrown." );
            }
            catch( Exception exception )
            {
                _log.Write( exception );
            }

            string[] fields = LogWriterTests._parseLineFromFile( _log.FileLocation );

            DateTime.Parse( fields[ 0 ] ); // NOTE: Test the date/time is at least valid (it's tough to actually check the date/time value)
            // NOTE: The following ensures that the exception expression at least starts the same as we expect.  The reason we can't compare 
            // the entire string is because the debug symbols will vary depending on where the source was built.
            Assert.IsTrue( fields[ 1 ].StartsWith( "System.ApplicationException: An exception was thrown.\x1A   at _unittests.org.ncore.Diagnostics.LogWriterTests.Write_WithException()" ) );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write_WithException()" );
            Assert.AreEqual( fields[ 3 ], "Exception" );
            Assert.AreEqual( fields[ 4 ], "0" );
        }
        
        [TestMethod]
        public void Write_WithExceptionAndEntryType()
        {
            _log.LogTarget = _log.LogTarget | LogWriter.LogTargetEnum.EventLog;
            try
            {
                throw new ApplicationException( "An exception was thrown." );
            }
            catch( Exception exception )
            {
                _log.Write( exception, LogWriter.EntryTypeEnum.Information );
            }

            string[] fields = LogWriterTests._parseLineFromFile( _log.FileLocation );

            DateTime.Parse( fields[ 0 ] ); // NOTE: Test the date/time is at least valid (it's tough to actually check the date/time value)
            // NOTE: The following ensures that the exception expression at least starts the same as we expect.  The reason we can't compare 
            // the entire string is because the debug symbols will vary depending on where the source was built.
            Assert.IsTrue( fields[ 1 ].StartsWith( "System.ApplicationException: An exception was thrown.\x1A   at _unittests.org.ncore.Diagnostics.LogWriterTests.Write_WithExceptionAndEntryType()" ) );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write_WithExceptionAndEntryType()" );
            Assert.AreEqual( fields[ 3 ], "Information" );
            Assert.AreEqual( fields[ 4 ], "0" );
        }

        [TestMethod]
        public void Write_CustomizedLogFileDetails()
        {
            _log.FileExtension = "txt";
            _log.FileNameDateFormat = "MM-dd-yyyy";
            _log.FileNameSuffix = "_log";
            _log.FileNamePrefix = "unit-test_";
            _log.FilePath = System.Environment.CurrentDirectory + @"\testlog\";

            using( _log )
            {
                _log.Write( "Write_CustomizedLogFileDetails" );
                _log.Write( "Write_CustomizedLogFileDetails" );
                _log.Write( "Write_CustomizedLogFileDetails" );
            }
            // UNDONE: Figure out this whole .FileName / .LogFileFullName - I mean seriously, WTF?!
            string[] fields = LogWriterTests._parseLineFromFile( _log.FileLocation );

            Assert.IsTrue( _log.FileName.StartsWith( "unit-test_" ) );
            Assert.IsTrue( _log.FileName.EndsWith( "_log.txt" ) );
            Assert.IsTrue( _log.FileName.Substring( 10, 10 ) == DateTime.Now.ToString( "MM-dd-yyyy" ) );

            DateTime.Parse( fields[ 0 ] ); // Test the date/time is at least valid (it's tough to actually check the date/time value)
            Assert.AreEqual( fields[ 1 ], "Write_CustomizedLogFileDetails" );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write_CustomizedLogFileDetails()" );
            Assert.AreEqual( fields[ 3 ], "Information" );
            Assert.AreEqual( fields[ 4 ], "0" );

            if( Directory.Exists( _log.FilePath ) )
            {
                Directory.Delete( _log.FilePath, true );
            }
        }

        [TestMethod]
        public void Write_ChangeLogFileDetailsBetweenEntries()
        {
            string initialLogFile = _log.FileLocation;

            _log.Write( "Write_ChangeLogFileDetailsBetweenEntries" );

            _log.FileExtension = "txt";
            _log.FileNameDateFormat = "MM-dd-yyyy";
            _log.FileNameSuffix = "_log";
            _log.FileNamePrefix = "unit-test_";
            _log.FilePath = System.Environment.CurrentDirectory + @"\testlog\";

            _log.Write( "Write_ChangeLogFileDetailsBetweenEntries" );

            // UNDONE: Figure out this whole .FileName / .LogFileFullName - I mean seriously, WTF?!
            string[] fields = LogWriterTests._parseLineFromFile( _log.FileLocation );

            Assert.IsTrue( _log.FileName.StartsWith( "unit-test_" ) );
            Assert.IsTrue( _log.FileName.EndsWith( "_log.txt" ) );
            Assert.IsTrue( _log.FileName.Substring( 10, 10 ) == DateTime.Now.ToString( "MM-dd-yyyy" ) );

            DateTime.Parse( fields[ 0 ] ); // Test the date/time is at least valid (it's tough to actually check the date/time value)
            Assert.AreEqual( fields[ 1 ], "Write_ChangeLogFileDetailsBetweenEntries" );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write_ChangeLogFileDetailsBetweenEntries()" );
            Assert.AreEqual( fields[ 3 ], "Information" );
            Assert.AreEqual( fields[ 4 ], "0" );

            if( Directory.Exists( _log.FilePath ) )
            {
                Directory.Delete( _log.FilePath, true );
            }

            fields = LogWriterTests._parseLineFromFile( initialLogFile );

            DateTime.Parse( fields[ 0 ] ); // Test the date/time is at least valid (it's tough to actually check the date/time value)
            Assert.AreEqual( fields[ 1 ], "Write_ChangeLogFileDetailsBetweenEntries" );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write_ChangeLogFileDetailsBetweenEntries()" );
            Assert.AreEqual( fields[ 3 ], "Information" );
            Assert.AreEqual( fields[ 4 ], "0" );
        }

        // TODO: See comments on EventLog Source creation in TestFixture setup.  JF
        // TODO: The whole "Source" thing is a bit confusing as well since it means something different for the EventLog than it does for the LogWriter.  JF
        [TestMethod]
        //[Ignore]
        public void Write_ToEventLog()
        {
            _log.LogTarget = LogWriter.LogTargetEnum.EventLog;
            _log.EventLogSource = _eventLogSource;

            _log.Write( "Write_ToEventLog", LogWriter.EntryTypeEnum.Error );

            EventLog eventLog = new EventLog( "Application" );

            // HACK: Assumes that the entry we're interested in is the last one
            //  with this source in the list.  Probably true but still hacky.
            //  I don't really know how to deal with this otherwise though.  JF
            // HACK: Oh, and btw, this approach of spinning through EVERYTHING
            //  in the event log to find our entry is SOOOO slow.  Again, I'm 
            //  stumped as to a better alternative.  JF
            List<EventLogEntry> entries = new List<EventLogEntry>();
            foreach( EventLogEntry entry in eventLog.Entries )
            {
                if( entry.Source == _eventLogSource )
                {
                    entries.Add( entry );
                }
            }
            EventLogEntry targetEntry = entries.Last();

            Assert.IsNotNull( targetEntry );
            Assert.AreEqual( targetEntry.Message, "In _unittests.org.ncore.Diagnostics.LogWriterTests->Write_ToEventLog():\r\nWrite_ToEventLog" );
            Assert.AreEqual( targetEntry.Source, _eventLogSource );
            Assert.AreEqual( targetEntry.EntryType, EventLogEntryType.Error );
            Assert.AreEqual( targetEntry.InstanceId, 0 );
        }

        // TODO: See event log issues mentioned above in Write_ToEventLog() test.  JF
        [TestMethod]
        //[Ignore]
        public void Write_ToEventLogWithAdditionalEntryData()
        {
            _log.LogTarget = LogWriter.LogTargetEnum.EventLog;
            _log.EventLogSource = _eventLogSource;

            _log.Write( "Write_ToEventLogWithAdditionalEntryData", LogWriter.EntryTypeEnum.Error, 0, "A=1", "B=2", "C=3" );

            EventLog eventLog = new EventLog( "Application" );

            List<EventLogEntry> entries = new List<EventLogEntry>();
            foreach( EventLogEntry entry in eventLog.Entries )
            {                
                if( entry.Source == _eventLogSource )
                {
                    entries.Add( entry );
                }
            }
            EventLogEntry targetEntry = entries.Last();

            Assert.IsNotNull( targetEntry );
            Assert.AreEqual( targetEntry.Message, "In _unittests.org.ncore.Diagnostics.LogWriterTests->Write_ToEventLogWithAdditionalEntryData():\r\nWrite_ToEventLogWithAdditionalEntryData" );
            Assert.AreEqual( targetEntry.Source, _eventLogSource );
            Assert.AreEqual( targetEntry.EntryType, EventLogEntryType.Error );
            Assert.AreEqual( targetEntry.InstanceId, 0 );
            string additionalEntryData = targetEntry.Data.ToText();
            Assert.AreEqual( additionalEntryData, "A=1\u001FB=2\u001FC=3" );
        }

        [TestMethod]
        public void Write_UsingDerivedLogWriter()
        {
            LogWriter log = new DerivedLogWriter();

            using( log )
            {
                log.Write( "Write_UsingDerivedLogWriter" );
                log.Write( "Write_UsingDerivedLogWriter" );
                log.Write( "Write_UsingDerivedLogWriter" );
            }
            // UNDONE: Figure out this whole .FileName / .LogFileFullName - I mean seriously, WTF?!
            string[] fields = LogWriterTests._parseLineFromFile( log.FileLocation );

            Assert.IsTrue( log.FileName.StartsWith( "unit-test_" ) );
            Assert.IsTrue( log.FileName.EndsWith( "_log.txt" ) );
            Assert.IsTrue( log.FileName.Substring( 10, 10 ) == DateTime.Now.ToString( "MM-dd-yyyy" ) );

            DateTime.Parse( fields[ 0 ] ); // Test the date/time is at least valid (it's tough to actually check the date/time value)
            Assert.AreEqual( fields[ 1 ], "Write_UsingDerivedLogWriter" );
            Assert.AreEqual( fields[ 2 ], "_unittests.org.ncore.Diagnostics.LogWriterTests->Write_UsingDerivedLogWriter()" );
            Assert.AreEqual( fields[ 3 ], "Information" );
            Assert.AreEqual( fields[ 4 ], "0" );

            if( Directory.Exists( log.FilePath ) )
            {
                Directory.Delete( log.FilePath, true );
            }
        }

        [TestMethod]
        public void Write_ToConsole()
        {
            _log.LogTarget = LogWriter.LogTargetEnum.Console;

            MemoryStream stream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter( stream )
                                            {
                                                AutoFlush = true,
                                            };
            Console.SetOut( streamWriter );

            using( _log )
            {
                _log.Write( "First entry" );
                _log.Write( "Second entry" );
                _log.Write( "Third (and last) entry" );
            }

            string line = LogWriterTests._readLineFromStream( stream, 1 );
            // TODO: Implement regex for DateTime at the end of the line.  JF
            Assert.IsTrue( line.StartsWith( "First entry [ADDITIONAL DATA: ] [SOURCE: _unittests.org.ncore.Diagnostics.LogWriterTests->Write_ToConsole()] [ENTRY TYPE: Information] [EVENT NUMBER: 0] [DATETIME: " ) );
            line = LogWriterTests._readLineFromStream( stream, 2 );
            Assert.IsTrue( line.StartsWith( "Second entry [ADDITIONAL DATA: ] [SOURCE: _unittests.org.ncore.Diagnostics.LogWriterTests->Write_ToConsole()] [ENTRY TYPE: Information] [EVENT NUMBER: 0] [DATETIME: " ) );
            line = LogWriterTests._readLineFromStream( stream, 3 );
            Assert.IsTrue( line.StartsWith( "Third (and last) entry [ADDITIONAL DATA: ] [SOURCE: _unittests.org.ncore.Diagnostics.LogWriterTests->Write_ToConsole()] [ENTRY TYPE: Information] [EVENT NUMBER: 0] [DATETIME: " ) );

            stream.Close();
        }

        [TestMethod]
        public void Write_ToConsoleAndFile()
        {
            _log.LogTarget = _log.LogTarget | LogWriter.LogTargetEnum.Console;
            // OR:
            //_log.LogTarget = LogWriter.LogTargetEnum.File | LogWriter.LogTargetEnum.Console;

            MemoryStream stream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter( stream )
            {
                AutoFlush = true,
            };
            Console.SetOut( streamWriter );

            using( _log )
            {
                _log.Write( "First entry" );
                _log.Write( "Second entry" );
                _log.Write( "Third (and last) entry" );
            }

            string line = LogWriterTests._readLineFromStream( stream, 1 );
            // TODO: Implement regex for DateTime at the end of the line.  JF
            Assert.IsTrue( line.StartsWith( "First entry [ADDITIONAL DATA: ] [SOURCE: _unittests.org.ncore.Diagnostics.LogWriterTests->Write_ToConsoleAndFile()] [ENTRY TYPE: Information] [EVENT NUMBER: 0] [DATETIME: " ) );
            line = LogWriterTests._readLineFromStream( stream, 2 );
            Assert.IsTrue( line.StartsWith( "Second entry [ADDITIONAL DATA: ] [SOURCE: _unittests.org.ncore.Diagnostics.LogWriterTests->Write_ToConsoleAndFile()] [ENTRY TYPE: Information] [EVENT NUMBER: 0] [DATETIME: " ) );
            line = LogWriterTests._readLineFromStream( stream, 3 );
            Assert.IsTrue( line.StartsWith( "Third (and last) entry [ADDITIONAL DATA: ] [SOURCE: _unittests.org.ncore.Diagnostics.LogWriterTests->Write_ToConsoleAndFile()] [ENTRY TYPE: Information] [EVENT NUMBER: 0] [DATETIME: " ) );

            stream.Close();
        }

        [TestMethod]
        // TODO: How do we peek at the EventLog and ensure that we're NOT writing there.  That's a bit of a unit testing trick, no?  JF
        public void Write_LogTargetNull()
        {
            _log.LogTarget = null;

            MemoryStream stream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter( stream )
            {
                AutoFlush = true,
            };
            Console.SetOut( streamWriter );

            using( _log )
            {
                _log.Write( "First entry" );
                _log.Write( "Second entry" );
                _log.Write( "Third (and last) entry" );
            }

            Assert.AreEqual( 0, stream.Length );
            Assert.IsFalse( File.Exists( _log.FileLocation ) );

            stream.Close();
        }


        [TestMethod]
        // TODO: How do we peek at the EventLog and ensure that we're NOT writing there.  That's a bit of a unit testing trick, no?  JF
        public void Write_EntryTypesNull()
        {
            _log.LogTarget = _log.LogTarget | LogWriter.LogTargetEnum.Console;
            _log.FileEntryTypes = null;
            _log.ConsoleEntryTypes = null;
            _log.EventLogEntryTypes = null;

            MemoryStream stream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter( stream )
            {
                AutoFlush = true,
            };
            Console.SetOut( streamWriter );

            using( _log )
            {
                _log.Write( "Error", LogWriter.EntryTypeEnum.Error );
                _log.Write( "FailureAudit", LogWriter.EntryTypeEnum.FailureAudit );
                _log.Write( "Information", LogWriter.EntryTypeEnum.Information );
                _log.Write( "SuccessAudit", LogWriter.EntryTypeEnum.SuccessAudit );
                _log.Write( "Exception", LogWriter.EntryTypeEnum.Exception );
                _log.Write( "Warning", LogWriter.EntryTypeEnum.Warning );
            }

            Assert.AreEqual( 0, stream.Length );
            Assert.IsFalse( File.Exists( _log.FileLocation ) );

            stream.Close();
        }

        // NOTE: See TODO above regarding multi-threaded unit tests for an explanation of why this is commented out.
        /*
        [TestMethod]
        public void Write_MultiThreaded()
        {
            LogCaller logCaller = new LogCaller();

            LogCallerDelegate altLogCaller = new LogCallerDelegate( logCaller.DoLogging );
            IAsyncResult asyncResult = altLogCaller.BeginInvoke( __log, null, null );

            for( int i = 0; i < 1000; i++ )
            {
                DateTime dt = DateTime.Now;
                string ticks = dt.Ticks.ToString();
                __log.Write( "Primary - logging #" + i.ToString() + " " + ticks );
            }

            asyncResult.AsyncWaitHandle.WaitOne( 10000, false );

            if( asyncResult.IsCompleted )
            {
                altLogCaller.EndInvoke( asyncResult );
            }
        }
        */
    }

    // NOTE: Example of a simple derived "self-configuring" LogWriter.  JF
    public class DerivedLogWriter : LogWriter
    {
        protected override string defineFileExtension()
        {
            return "txt";
        }

        protected override string defineFileNameDateFormat()
        {
            return "MM-dd-yyyy";
        }

        protected override string defineFileNameSuffix()
        {
            return "_log";
        }

        protected override string defineFileNamePrefix()
        {
            return "unit-test_";
        }

        protected override string defineFilePath()
        {
            return System.Environment.CurrentDirectory + @"\derivedlog\";
        }
    }

    // TODO: Hmm.  Need to work this out.  JF
    // NOTE: See TODO above regarding multi-threaded unit tests and Write_MultiThreaded() for an explanation of why this is commented out.
    /*
    public delegate void LogCallerDelegate( LogWriter log );

    public class LogCaller
    {
        public void DoLogging( LogWriter log )
        {
            for( int i = 0; i < 1000; i++ )
            {
                DateTime dt = DateTime.Now;
                string ticks = dt.Ticks.ToString();
                log.Write( "LogCaller - logging #" + i.ToString() + " " + ticks );
            }
        }
    }
    */
}
