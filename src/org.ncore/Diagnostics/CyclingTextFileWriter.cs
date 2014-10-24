using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using org.ncore.Common;
using org.ncore.Extensions;

namespace org.ncore.Diagnostics
{
    // TODO: Unit tests!!! JKF
    public class CyclingTextFileWriter
    {
        public class IntervalUnitFactype : Factype
        {
            public static readonly IntervalUnitFactype Seconds = new IntervalUnitFactype( "yyyy-MM-dd HH_mm_" );
            public static readonly IntervalUnitFactype Minutes = new IntervalUnitFactype( "yyyy-MM-dd HH_" );
            public static readonly IntervalUnitFactype Hours = new IntervalUnitFactype( "yyyy-MM-dd " );
            public static readonly IntervalUnitFactype Days = new IntervalUnitFactype( "yyyy-MM-" );

            private IntervalUnitFactype( string baseFormatMask )
                : base()
            {
                this.BaseFormatMask = baseFormatMask;
            }

            // NOTE: Optional additions to the class to add functionality.  These are all just wrapper methods.  JF
            public static implicit operator IntervalUnitFactype( string moniker )
            {
                return IntervalUnitFactype.Parse<IntervalUnitFactype>( moniker );
            }

            public static List<IntervalUnitFactype> GetAll()
            {
                return IntervalUnitFactype.GetAll<IntervalUnitFactype>();
            }

            public static string[] GetMonikers()
            {
                return IntervalUnitFactype.GetMonikers<IntervalUnitFactype>();
            }

            public static string[] GetBaseFormatMasks()
            {
                List<IntervalUnitFactype> list = IntervalUnitFactype.GetAll();
                string[] masks = new string[list.Count];
                for( int i = 0; i < masks.Length; ++i )
                {
                    masks[i] = list[i].BaseFormatMask;
                }
                return masks;
            }

            public string BaseFormatMask { get; protected set; }
        }

        private TextFileWriter _textFileWriter;

        private int _interval = 1;
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        private IntervalUnitFactype _intervalUnit = IntervalUnitFactype.Hours;
        public IntervalUnitFactype IntervalUnit
        {
            get { return _intervalUnit; }
            set { _intervalUnit = value; }
        }

        private string _targetDirectory = string.Empty;
        public string TargetDirectory
        {
            get { return _targetDirectory; }
            set { _targetDirectory = value; }
        }

        private string _fileExtension = "trace";
        public string FileExtension
        {
            get { return _fileExtension; }
            set { _fileExtension = value; }
        }

        public CyclingTextFileWriter()
        {
            _textFileWriter = new TextFileWriter();
        }

        public void Write( string message )
        {
            _configureFileLocation();
            _textFileWriter.Write( message );
        }

        public void WriteLine( string message )
        {
            _configureFileLocation();
            _textFileWriter.WriteLine( message );
        }

        private void _configureFileLocation()
        {
            DateTime now = DateTime.Now;

            int current;
            if( this.IntervalUnit == IntervalUnitFactype.Seconds )
            {
                current = now.Second;
            }
            else if( this.IntervalUnit == IntervalUnitFactype.Minutes )
            {
                current = now.Minute;
            }
            else if( this.IntervalUnit == IntervalUnitFactype.Hours )
            {
                current = now.Hour;
            }
            else if( this.IntervalUnit == IntervalUnitFactype.Days )
            {
                current = now.Day;
            }
            else
            {
                // NOTE: This should never happen unless we define a new IntervalUnitFactype value.  JKF
                throw new ApplicationException( "Unknown IntervalUnit value." );
            }

            int quantized = current - ( current % this.Interval );

            string quantizedText = quantized.ToString( CultureInfo.InvariantCulture ).PadLeft( 2, '0' );

            StringBuilder builder = new StringBuilder();
            builder.Append( now.ToString( this.IntervalUnit.BaseFormatMask ) );
            builder.Append( quantizedText );
            builder.Append( "." );
            builder.Append( this.FileExtension );
            string fileName = builder.ToString();

            string path = Path.Combine( this.TargetDirectory, fileName );

            _textFileWriter.FileLocation = Path.GetFullPath( path );
        }
    }
}
