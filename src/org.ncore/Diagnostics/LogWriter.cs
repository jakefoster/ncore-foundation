// TODO: Add "custom log target" interface and collection in LogWriter to allow the creation of derived, specialized logging components (e.g. XML file).  Really?!  JF
// TODO: Need to address the issues around writing to the EventLog and the fact that it basically doesn't work without manual permission tweaks in the registry.  See unit tests for detailed comments.  JF
// TODO: Need more unit tests for writing to console and event log.  JF

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using org.ncore.Extensions;

namespace org.ncore.Diagnostics
{
	// TODO: Add xml comments for class
	public class LogWriter : IDisposable
	{
		const int MAX_EVENTLOG_TEXT_SIZE = 2048;

        // GENERAL GUIDANCE ON ENTRYTYPES
        //  This is not meant to be definitive.  These events can mean whatever you want them to!
        // 
        //  Information: General informational event
        //  Warning = Represents a logical warning state or event
        //  Error:  Represents a logical error state or event (though not necessarily an exception)
        //  Exception: Should be used only when exception has been thrown
        //
        //  SuccessAudit: Detailed data, state or condition of logical "success" event (possibly relating to an "Information" event)
        //  FailureAudit: Detailed data, state or condition of logical "failure" event (possibly relating to a "Warning", "Error" or "Exception" event)
        [FlagsAttribute]
		public enum EntryTypeEnum : byte
		{
			Error = 1,
			Warning = 2,
			Information = 4,
			SuccessAudit = 8,
			FailureAudit = 16,
			Exception = 32
		}

		[FlagsAttribute]
		public enum LogTargetEnum : byte
		{
			File = 1,
			EventLog = 2,
			Console = 4
		}

		public LogWriter()
		{
            _textFileWriter = new TextFileWriter();
			_initializeConfiguration();
		}

        private LogTargetEnum? _logTarget = LogTargetEnum.File; // NOTE: Default is to only log to file.
		private EntryTypeEnum? _fileEntryTypes = (EntryTypeEnum)0x3F; // NOTE: 0x3F is the equivalent of all entry types.
		private EntryTypeEnum? _eventLogEntryTypes = (EntryTypeEnum)0x21; // NOTE: 0x21 is the equivalent of Error and Exception entry types.
		private EntryTypeEnum? _consoleEntryTypes = (EntryTypeEnum)0x3F; // NOTE: 0x3F is the equivalent of all entry types.
		private string _filePath = Environment.CurrentDirectory;
		private string _fileNamePrefix = string.Empty;
		private string _fileNameDateFormat = "yyyyMMdd";
		private string _fileNameSuffix = string.Empty;
		private string _fileExtension = "log";
		private string _explicitFileName = string.Empty;
		private string _delimiter = "\x1F"; // NOTE: This is the 'Unit Separator' character in the ASCII character set.		
		private bool _sharedWriteAccess = true;
		private string _newLineReplacement = "\x1A"; // NOTE: This is the 'Substitution' character in the ASCII character set.		
        private string _eventLogSource = string.Empty;
		private string _constructedFileName = string.Empty;
		private string _fileLocation = string.Empty;
		
	    private TextFileWriter _textFileWriter = null;

        public LogTargetEnum? LogTarget
		{
			get{ return _logTarget; }
			set{ _logTarget = value; }
		}

		public EntryTypeEnum? FileEntryTypes
		{
			get{ return _fileEntryTypes; }
			set{ _fileEntryTypes = value; }
		}

		public EntryTypeEnum? EventLogEntryTypes
		{
			get{ return _eventLogEntryTypes; }
			set{ _eventLogEntryTypes = value; }
		}

		public EntryTypeEnum? ConsoleEntryTypes
		{
			get{ return _consoleEntryTypes; }
			set{ _consoleEntryTypes = value; }
		}

		public string FilePath
		{
			get{ return _filePath; }
			set
			{
				_filePath = Path.GetFullPath( value );
				_initializeFileConfiguration();
			}
		}

		public string FileNamePrefix
		{
			get{ return _fileNamePrefix; }
			set
			{
				_fileNamePrefix = value;
				_initializeFileConfiguration();
			}
		}

		public string FileNameDateFormat
		{
			get{ return _fileNameDateFormat; }
			set
			{
				_fileNameDateFormat = value;
				_initializeFileConfiguration();
			}
		}

		public string FileNameSuffix
		{
			get{ return _fileNameSuffix; }
			set
			{
				_fileNameSuffix = value;
				_initializeFileConfiguration();
			}
		}

		public string FileExtension
		{
			get{ return _fileExtension; }
			set
			{
				_fileExtension = value;
				_initializeFileConfiguration();
			}
		}

		public string FileName
		{
			get{ return _constructedFileName; }
			set
			{
				_explicitFileName = value;
				_initializeFileConfiguration();
			}
		}

		public string FileLocation
		{
			get{ return _fileLocation; }
		}
		
		public string Delimiter
		{
			get{ return _delimiter; }
			set
			{
				_delimiter = value;
			}
		}

		public bool SharedWriteAccess
		{
			get{ return _sharedWriteAccess; }
			set
            { 
                _sharedWriteAccess = value;
                _textFileWriter.SharedWriteAccess = value;
            }
		}

		public string NewLineReplacement
		{
			get{ return _newLineReplacement; }
			set{ _newLineReplacement = value; }
		}

	    public string EventLogSource
        {
            get { return _eventLogSource; }
            set { _eventLogSource = value; }
	    }

        public void Dispose()
		{
            if( _textFileWriter != null )
            {
                _textFileWriter.Dispose();
            }
		}

		public void Write( string message, params string[] additionalEntryData )
		{
			_write( message, _enumerateSource( 2 ), EntryTypeEnum.Information, 0, additionalEntryData );
		}

		public void Write( string message )
		{
			_write( message, _enumerateSource( 2 ), EntryTypeEnum.Information, 0, null );
		}

		public void Write( string message, EntryTypeEnum entryType )
		{
			_write( message, _enumerateSource( 2 ), entryType, 0, null );
		}

		public void Write( string message, EntryTypeEnum entryType, ushort eventNumber )
		{
			_write( message, _enumerateSource( 2 ), entryType, eventNumber, null );
		}

        public void Write( string message, EntryTypeEnum entryType, params string[] additionalEntryData )
        {
            _write( message, _enumerateSource( 2 ), entryType, 0, additionalEntryData );
        }

		public void Write( string message, EntryTypeEnum entryType, ushort eventNumber, params string[] additionalEntryData )
		{
			_write( message, _enumerateSource( 2 ), entryType, eventNumber, additionalEntryData );
		}

        public void Write( string message, string source, EntryTypeEnum entryType )
		{
			_write( message, source, entryType, 0, null );
		}

		public void Write( string message, string source, EntryTypeEnum entryType, ushort eventNumber )
		{
			_write( message, source, entryType, eventNumber, null );
		}

		public void Write( string message, string source, EntryTypeEnum entryType, ushort eventNumber, params string[] additionalEntryData )
		{
			_write( message, source, entryType, eventNumber, additionalEntryData );
		}

		public void Write( Exception exception )
		{
			_write( exception.ToString(), _enumerateSource( 2 ), EntryTypeEnum.Exception, 0, (string[])null );
		}

		public void Write( Exception exception, EntryTypeEnum entryTypeEnum )
		{
			_write( exception.ToString(), _enumerateSource( 2 ), entryTypeEnum, 0, (string[])null );
		}

        public void Write( Exception exception, params string[] additionalEntryData )
        {
            _write( exception.ToString(), _enumerateSource( 2 ), EntryTypeEnum.Exception, 0, additionalEntryData );
        }

        public void Write( Exception exception, EntryTypeEnum entryTypeEnum, params string[] additionalEntryData )
        {
            _write( exception.ToString(), _enumerateSource( 2 ), entryTypeEnum, 0, additionalEntryData );
        }

        protected virtual LogTargetEnum? defineLogTarget()
		{
			return _logTarget;
		}

		protected virtual EntryTypeEnum? defineFileEntryTypes()
		{
			return _fileEntryTypes;
		}	

		protected virtual EntryTypeEnum? defineEventLogEntryTypes()
		{
			return _eventLogEntryTypes;
		}

		protected virtual EntryTypeEnum? defineConsoleEntryTypes()
		{
			return _consoleEntryTypes; 
		}

		protected virtual string defineFilePath()
		{
			return _filePath; 
		}
		
		protected virtual string defineFileNamePrefix()
		{
			return _fileNamePrefix; 
		}

		protected virtual string defineFileNameDateFormat()
		{
			return _fileNameDateFormat; 
		}

		protected virtual string defineFileNameSuffix()
		{
			return _fileNameSuffix;
		}

		protected virtual string defineFileExtension()
		{
			return _fileExtension;
		}

		protected virtual string defineFileName()
		{
			return _explicitFileName; 
		}
		
		protected virtual string defineDelimiter()
		{
			return _delimiter; 
		}

		protected virtual bool defineSharedWriteAccess()
		{
			return _sharedWriteAccess; 
		}

		protected virtual string defineNewLineReplacement()
		{
			return _newLineReplacement; 
		}

        protected virtual string defineEventLogSource()
        {
            return _eventLogSource;
        }

        private void _initializeConfiguration()
		{
			_logTarget = defineLogTarget();
			_fileEntryTypes = defineFileEntryTypes();
			_eventLogEntryTypes = defineEventLogEntryTypes();
			_consoleEntryTypes = defineConsoleEntryTypes();
			_filePath = defineFilePath();
			_fileNamePrefix = defineFileNamePrefix();
			_fileNameDateFormat = defineFileNameDateFormat();
			_fileNameSuffix = defineFileNameSuffix();
			_fileExtension = defineFileExtension();
			_explicitFileName = defineFileName();
			_delimiter = defineDelimiter();
			_sharedWriteAccess = defineSharedWriteAccess();
			_newLineReplacement = defineNewLineReplacement();
            _eventLogSource = defineEventLogSource();

			_initializeFileConfiguration();
		}

        // TODO: This is a little weird.  If .FileName is set it uses that, otherwise it constructs it
        //  from .FileNamePrefix, .FileNameSuffix, etc.  Could be confusing if you set all those
        //  properties but .FileName has a value so you get that as the filename instead of a name
        //  constructed from the component parts.  Should setting .FileName wipe out those other 
        //  properties and vice versa?  JF
        //  UPDATE: Umm, that might actually be how it works.  I guess I should write some tests <blush>.  JF
		private void _initializeFileConfiguration()
		{
			StringBuilder builder = new StringBuilder();
            if( !string.IsNullOrEmpty( _explicitFileName ) )
			{
				builder.Append( _explicitFileName );
			}
			else
			{
				builder.Append( _fileNamePrefix );
				builder.Append( DateTime.Now.ToString( _fileNameDateFormat ) );
				builder.Append( _fileNameSuffix );
				if( _fileExtension != string.Empty )
				{
					builder.Append( "." );
					builder.Append( _fileExtension );
				}
			}
			_constructedFileName = builder.ToString();
            _fileLocation = Path.Combine( _filePath, _constructedFileName ).ToString();

		    _textFileWriter.FileLocation = _fileLocation;
		}

		private static string _enumerateSource( int stackFrame )
		{
			try
			{
				MethodBase method = new StackFrame( stackFrame, false ).GetMethod();
				string source = method.DeclaringType.FullName + "->" + method.Name.ToString();
				if( method.MemberType == System.Reflection.MemberTypes.Method || method.MemberType == System.Reflection.MemberTypes.Constructor )
				{
					source += "()";
				}
				return source;
			}
			catch{}
			return "Unknown";
		}
        
		private string _formatEntry( string dateTimeStamp, string message, string source, string entryType, string eventNumber, params string[] additionalEntryData )
		{
			string[] components = new String[5] { dateTimeStamp, message.Replace( System.Environment.NewLine, _newLineReplacement ), source, entryType, eventNumber };
			if( additionalEntryData != null )
			{
				int arraySize = 5 + additionalEntryData.Length;
				ArrayList entryElements = new ArrayList( arraySize );
				entryElements.AddRange( components );
				foreach( string additionalDataItem in additionalEntryData )
				{
					entryElements.Add( additionalDataItem.Replace( System.Environment.NewLine, _newLineReplacement ) );
				}
				components = (string[])entryElements.ToArray( typeof( string ) );
			}
			return String.Join( _delimiter, components );
		}

        // TODO: Cleanup!  JF
		private string _formatReadableEntry( string dateTimeStamp, string message, string source, string entryType, string eventNumber, params string[] additionalEntryData )
		{
            // TODO: Which version do we like better here?  JF
			/*
			string[] components = new String[5] { dateTimeStamp, message.Replace( System.Environment.NewLine, _newLineReplacement ), source, entryType, eventNumber };
			if( additionalEntryData != null )
			{
				int arraySize = 5 + additionalEntryData.Length;
				ArrayList entryElements = new ArrayList( arraySize );
				entryElements.AddRange( components );
				foreach( string additionalDataItem in additionalEntryData )
				{
					entryElements.Add( additionalDataItem.Replace( System.Environment.NewLine, _newLineReplacement ) );
				}
				components = (string[])entryElements.ToArray( typeof( string ) );
			}
			return String.Join( _delimiter, components );
			*/

			StringBuilder builder = new StringBuilder();
			builder.Append( message.Replace( _delimiter, "+").Replace( System.Environment.NewLine, _newLineReplacement ) );
			builder.Append(" [ADDITIONAL DATA: ");
			if( additionalEntryData != null )
			{
				foreach( string additionalDataItem in additionalEntryData )
				{
					builder.Append(" (");
					builder.Append( additionalDataItem.Replace( _delimiter, "+" ).Replace( System.Environment.NewLine, _newLineReplacement ) );
					builder.Append(") ");
				}
			}
			builder.Append( "] [SOURCE: " );
			builder.Append( source.Replace( _delimiter, "+" ) );
			builder.Append( "] [ENTRY TYPE: " );
			builder.Append( entryType.ToString() );
			builder.Append( "] [EVENT NUMBER: " );
			builder.Append( eventNumber.ToString() );
			builder.Append( "] [DATETIME: " );
			builder.Append( dateTimeStamp );
			builder.Append( "]" );

			return builder.ToString();
		}

		private void _write( string message, string source, EntryTypeEnum entryType, ushort eventNumber, params string[] additionalEntryData )
		{
            if( _logTarget == null )
            {
                return;
            }
			// NOTE: Be very careful when altering this code.  Unless you know what you're doing it's best 
			// to not change any of the core functionality within this method (or other methods called from
			// within this method.  It's been coded very carefully to be as performant as possible while
			// still being thread-safe and resource sensitive.
			lock( this )
			{
				string dateTimeStamp = DateTime.Now.ToString();

				// NOTE: Log to file
				if( ( _logTarget & LogTargetEnum.File ) == LogTargetEnum.File && ( _fileEntryTypes & entryType ) == entryType )
				{
					// NOTE: The log file is not held open.  On each call it is re-opened, written to, then closed and disposed of.
					// This is required to prevent concurrency/blocking issues on the physical file.  
					_textFileWriter.WriteLine( _formatEntry( dateTimeStamp, message, source, entryType.ToString(), eventNumber.ToString(), additionalEntryData ) );
				}

				// NOTE: Log to EventLog
                // TODO: This should probably be factored out into a EventLogWriter class like TextFileWriter.  JF
				if( ( _logTarget & LogTargetEnum.EventLog ) == LogTargetEnum.EventLog && ( _eventLogEntryTypes & entryType ) == entryType )
				{
					EventLog eventLog = new EventLog( "Application" );
					try 
					{
						StringBuilder builder = new StringBuilder();
						// NOTE: For the EventLog we're going to join together the additionalEntryData segments
						// (separated by the specified delimiter) and put it in the "RawData" portion of the 
						// EventLog entry.
						string rawData = string.Empty;
						if( additionalEntryData != null )
						{
							rawData = String.Join( _delimiter, additionalEntryData );
						}

						eventLog.Source = _eventLogSource;

						// NOTE: The Windows Event Log has no discreet event type for "Exception" so if that's
						// the event type that was specified we'll change it to be reported as an "Error" in the 
						// Event Log entry.  This is sub-optimal but there's really no other solution.
						EventLogEntryType eventLogEntryType;
						if( entryType == EntryTypeEnum.Exception )
						{
							eventLogEntryType = EventLogEntryType.Error;
						}
						else
						{
							eventLogEntryType = (EventLogEntryType)entryType;
						}
						// NOTE: EventLog entries have a 32k limit on the event expression.  To prevent EventLog bloat we'll
						// limit the size of the expression to the value specified in MAX_EVENTLOG_TEXT_SIZE.
						// UNDONE: Should this be a configurable property of the log instead of a constant?
                        //string eventText = message.Substring( 0, Math.Min( message.Length, MAX_EVENTLOG_TEXT_SIZE ) );
					    string eventText = String.Format( "In {0}:\r\n{1}", source, message );
                        eventLog.WriteEntry( eventText.Substring( 0, Math.Min( eventText.Length, MAX_EVENTLOG_TEXT_SIZE ) ), eventLogEntryType, eventNumber, 0, rawData.ToByteArray() );
					}
					catch( Exception )
					{
						// TODO: Is there anything that needs to happen here?
					}
					finally 
					{
						eventLog.Close();
						eventLog = null;
					}
				}

				string readableEntry = string.Empty;
				// NOTE: Log to Console
				if( ( _logTarget & LogTargetEnum.Console ) == LogTargetEnum.Console && ( _consoleEntryTypes & entryType ) == entryType )
				{
					readableEntry = _formatReadableEntry( dateTimeStamp, message, source, entryType.ToString(), eventNumber.ToString(), additionalEntryData );
					// TODO: Should this really be inside of a throw-away try-catch block?
					try
					{
						Console.WriteLine( readableEntry );
					}
					catch{}
				}
			}
		}
	}
}


