// TODO: Fix FileName, FullName confusion.  How about "Location" for Path + Name values?

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using org.ncore.Common;
using org.ncore.Extensions;

namespace org.ncore.Diagnostics
{
	// TODO: Add xml comments for class
	public class TextFileWriter : IDisposable
	{
        private string _fileLocation;
        private string _lastFileLocation;
	    private bool _sharedWriteAccess;
        private StreamWriter _streamWriter = null;

        public TextFileWriter()
        {
        }

        public TextFileWriter( string fileLocation )
        {
            _fileLocation = Path.GetFullPath( fileLocation );
        }

        public string FileLocation
		{
            get { return _fileLocation; }
			set
			{
                _fileLocation = Path.GetFullPath( value );
			}
		}
        
        // TODO: Need to do some deeper investigation on the implications of allowing shared write access.  May not even want to allow it.  JF
		public bool SharedWriteAccess
		{
			get{ return _sharedWriteAccess; }
			set{ _sharedWriteAccess = value; }
		}

        public void Dispose()
		{
			if( _streamWriter != null )
			{
				// NOTE: Using a throw-away try catch here because we don't want any errors bubbling up here.  
				//	We're just being squeeky clean with this bit of code anyway.  It should be mostly 
				//	redundant.  This is just to try and keep things afloat under unsual or exceptional
				//	circumstances.
				try
				{
					_streamWriter.Close();
				}
				catch{}
				_streamWriter = null;
			}

			// NOTE: We have to do this to ensure that the underlying file-system resources are released.
            // TODO: Really?!?  JF
			System.GC.Collect( 2 );
		}

		public void Write( string message )
		{
			_write( message );
		}

        public void WriteLine( string message )
        {
            _writeLine( message );
        }


		private void _openFile()
		{
			// NOTE: This is probably redundant but is being done to ensure that the log wasn't left in open
			//	as a result of an exception being thrown during a previous .Write() attempt.
			if( _streamWriter != null )
			{
				_closeFile();
			}

            Condition.Assert( !string.IsNullOrEmpty( _fileLocation ), new ArgumentException( "Parameter cannot be empty or null.", "FileLocation" ) );

            if( _lastFileLocation != _fileLocation )
			{
                _lastFileLocation = _fileLocation;
                if( Directory.Exists( Path.GetDirectoryName( _fileLocation ) ) == false )
				{
                    Directory.CreateDirectory( Path.GetDirectoryName( _fileLocation ) );
				}
			}

            // TODO: I need to better understand this SharedWriteAccess issue and write some tests.  JF
			FileShare fileShare = FileShare.ReadWrite;
			if( this._sharedWriteAccess == false )
			{
				fileShare = FileShare.Read;
			}
            FileStream fileStream = File.Open( _fileLocation, FileMode.Append, FileAccess.Write, fileShare );
		
			_streamWriter = new StreamWriter( fileStream );
			_streamWriter.AutoFlush = true;
		}

		private void _closeFile()
		{
			if( _streamWriter != null )
			{
				// NOTE: If the stream has already been flushed and/or closed this will throw an exception so
				//	we need this throw-away try-catch block to handle that possibility.
				try
				{
					_streamWriter.Flush();
					_streamWriter.Close();
				}
				catch{}
				_streamWriter = null;
			}
		}

		private void _write( string message  )
		{
			// NOTE: Be very careful when altering this code.  Unless you know what you're doing it's best 
			// to not change any of the core functionality within this method (or other methods called from
			// within this method.  It's been coded very carefully to be as performant as possible while
			// still being thread-safe and resource sensitive.
            lock( this )
            {
                // NOTE: The log file is not held open.  On each call it is re-opened, written to, then closed and disposed of.
                // This is required to prevent concurrency/blocking issues on the physical file.  
                _openFile();
                _streamWriter.Write( message );
                _closeFile();
            }
		}

        private void _writeLine( string message )
        {
            // NOTE: Be very careful when altering this code.  Unless you know what you're doing it's best 
            // to not change any of the core functionality within this method (or other methods called from
            // within this method.  It's been coded very carefully to be as performant as possible while
            // still being thread-safe and resource sensitive.
            lock( this )
            {
                // NOTE: The log file is not held open.  On each call it is re-opened, written to, then closed and disposed of.
                // This is required to prevent concurrency/blocking issues on the physical file.  
                _openFile();
                _streamWriter.WriteLine( message );
                _closeFile();
            }
        }
	}
}


