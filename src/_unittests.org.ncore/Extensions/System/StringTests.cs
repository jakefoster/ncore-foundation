using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Extensions;
using org.ncore.Common;

namespace _unittests.org.ncore.Extensions
{
    /// <summary>
    /// Summary description for StringTests
    /// </summary>
    [TestClass]
    public class System_StringTests
    {
        public System_StringTests()
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
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void ToShortDateString()
        {
            string dateTime = "10/8/2004 9:54:54 PM";
            string formattedDateTime = dateTime.ToShortDateString();

            Assert.AreEqual( "10/8/2004", formattedDateTime );
        }

        [TestMethod]
        public void ToShortDateString_OnNullInput()
        {
            string dateTime = null;
            string formattedDateTime = dateTime.ToShortDateString();

            Assert.AreEqual( string.Empty, formattedDateTime );
        }

        [TestMethod]
        public void ToShortDateString_OnEmptyInput()
        {
            string dateTime = string.Empty;
            string formattedDateTime = dateTime.ToShortDateString();

            Assert.AreEqual( string.Empty, formattedDateTime );
        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void ToShortDateString_OnNonDateTimeInput()
        {
            string dateTime = "not a valid date/time";
            string formattedDateTime = dateTime.ToShortDateString();
        }

        [TestMethod]
        public void ToStandardDateString()
        {
            string dateTime = "10/8/2004 9:54:54 PM";
            string formattedDateTime = dateTime.ToStandardDateString();

            Assert.AreEqual( "Oct 08, 2004", formattedDateTime );
        }

        [TestMethod]
        public void ToStandardDateString_OnNullInput()
        {
            string dateTime = null;
            string formattedDateTime = dateTime.ToStandardDateString();

            Assert.AreEqual( string.Empty, formattedDateTime );
        }

        [TestMethod]
        public void ToStandardDateString_OnEmptyInput()
        {
            string dateTime = string.Empty;
            string formattedDateTime = dateTime.ToStandardDateString();

            Assert.AreEqual( string.Empty, formattedDateTime );
        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void ToStandardDateString_OnNonDateTimeInput()
        {
            string dateTime = "not a valid date/time";
            string formattedDateTime = dateTime.ToStandardDateString();
        }

        [TestMethod]
        public void ToStandardDateTimeString()
        {
            string dateTime = "10/8/2004 9:54:54 PM";
            string formattedDateTime = dateTime.ToStandardDateString();
            
            Assert.AreEqual( "Oct 08, 2004", formattedDateTime );
        }

        [TestMethod]
        public void ToStandardDateTimeString_OnNullInput()
        {
            string dateTime = null;
            string formattedDateTime = dateTime.ToStandardDateString();

            Assert.AreEqual( string.Empty, formattedDateTime );
        }

        [TestMethod]
        public void ToStandardDateTimeString_OnEmptyInput()
        {
            string dateTime = string.Empty;
            string formattedDateTime = dateTime.ToStandardDateString();

            Assert.AreEqual( string.Empty, formattedDateTime );
        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void ToStandardDateTimeString_OnNonDateTimeInput()
        {
            string dateTime = "not a valid date/time";
            string formattedDateTime = dateTime.ToStandardDateString();
        }

        [TestMethod]
        public void ToLongDateString()
        {
            string dateTime = "10/8/2004 9:54:54 PM";
            string formattedDateTime = dateTime.ToLongDateString();

            Assert.AreEqual( "October 08, 2004", formattedDateTime );
        }

        [TestMethod]
        public void ToLongDateString_OnNullInput()
        {
            string dateTime = null;
            string formattedDateTime = dateTime.ToLongDateString();

            Assert.AreEqual( string.Empty, formattedDateTime );
        }

        [TestMethod]
        public void ToLongDateString_OnEmptyInput()
        {
            string dateTime = string.Empty;
            string formattedDateTime = dateTime.ToLongDateString();

            Assert.AreEqual( string.Empty, formattedDateTime );
        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void ToLongDateString_OnNonDateTimeInput()
        {
            string dateTime = "not a valid date/time";
            string formattedDateTime = dateTime.ToLongDateString();
        }

        [TestMethod]
        public void ToLongDateTimeString()
        {
            string dateTime = "10/8/2004 9:54:54 PM";
            string formattedDateTime = dateTime.ToLongDateTimeString();

            Assert.AreEqual( "October 08, 2004 9:54 PM", formattedDateTime );
        }

        [TestMethod]
        public void ToLongDateTimeString_OnNullInput()
        {
            string dateTime = null;
            string formattedDateTime = dateTime.ToLongDateTimeString();

            Assert.AreEqual( string.Empty, formattedDateTime );
        }

        [TestMethod]
        public void ToLongDateTimeString_OnEmptyInput()
        {
            string dateTime = string.Empty;
            string formattedDateTime = dateTime.ToLongDateTimeString();

            Assert.AreEqual( string.Empty, formattedDateTime );
        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void ToLongDateTimeString_OnNonDateTimeInput()
        {
            string dateTime = "not a valid date/time";
            string formattedDateTime = dateTime.ToLongDateTimeString();
        }

        [TestMethod]
        public void NullToEmpty_OnNullValue()
        {
            string input = null;
            string result = input.NullToEmpty();

            Assert.IsNotNull( result );
            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void NullToEmpty_OnEmptyValue()
        {
            string input = string.Empty;
            string result = input.NullToEmpty();

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void NullToEmpty_OnNonEmptyNonNullValue()
        {
            string input = "non-empty, non-null value";
            string result = input.NullToEmpty();

            Assert.AreEqual( input, result );
        }

        [TestMethod]
        public void IsEmpty_OnEmpty()
        {
            string input = string.Empty;
            bool result = input.IsEmpty();

            Assert.AreEqual( true, result );
        }

        [TestMethod]
        public void IsEmpty_OnNull()
        {
            string input = null;
            bool result = input.IsEmpty();

            Assert.AreEqual( false, result );
        }

        [TestMethod]
        public void IsEmpty_OnNonEmptyNonNullValue()
        {
            string input = "non-empty, non-null value";
            bool result = input.IsEmpty();

            Assert.AreEqual( false, result );
        }

        [TestMethod]
        public void IsEmptyOrNull_OnEmpty()
        {
            string input = string.Empty;
            bool result = input.IsEmptyOrNull();

            Assert.AreEqual( true, result );
        }

        [TestMethod]
        public void IsEmptyOrNull_OnNull()
        {
            string input = null;
            bool result = input.IsEmptyOrNull();

            Assert.AreEqual( true, result );
        }

        [TestMethod]
        public void IsEmptyOrNull_OnNonEmptyNonNullValue()
        {
            string input = "non-empty, non-null value";
            bool result = input.IsEmptyOrNull();

            Assert.AreEqual( false, result );
        }

        [TestMethod]
        public void IsDateTime()
        {
            string input = "October 08, 2004 9:54 PM";
            bool result = input.IsDateTime();

            Assert.AreEqual( true, result );
        }

        [TestMethod]
        public void IsDateTime_OnValidDateOnly()
        {
            string input = "October 08, 2004";
            bool result = input.IsDateTime();

            Assert.AreEqual( true, result );
        }

        [TestMethod]
        public void IsDateTime_OnValidTimeOnly()
        {
            string input = "9:54 PM";
            bool result = input.IsDateTime();

            Assert.AreEqual( true, result );
        }

        [TestMethod]
        public void IsDateTime_OnEmpty()
        {
            string input = string.Empty;
            bool result = input.IsDateTime();

            Assert.AreEqual( false, result );
        }

        [TestMethod]
        public void IsDateTime_OnNull()
        {
            string input = null;
            bool result = input.IsDateTime();

            Assert.AreEqual( false, result );
        }

        [TestMethod]
        public void IsDateTime_OnInvalidValue()
        {
            string input = "not a valid date/time value";
            bool result = input.IsDateTime();

            Assert.AreEqual( false, result );
        }

        [TestMethod]
        public void Tabify()
        {
            string sourceText = "This is some text to be tabified.";
            string result = sourceText.Tabify( 1 );

            Assert.AreEqual( "\tThis is some text to be tabified.", result );
        }

        [TestMethod]
        public void Tabify_CountOf0()
        {
            string sourceText = "This is some text to be tabified.";
            string result = sourceText.Tabify( 0 );

            Assert.AreEqual( "This is some text to be tabified.", result );
        }

        [TestMethod]
        public void Tabify_EmptySourceText()
        {
            string sourceText = string.Empty;
            string result = sourceText.Tabify( 1 );

            Assert.AreEqual( "\t", result );
        }

        [TestMethod]
        public void Tabify_NullSourceText()
        {
            string sourceText = null;
            string result = sourceText.Tabify( 1 );

            Assert.AreEqual( "\t", result );
        }

        [TestMethod]
        public void AddQuotes()
        {
            string input = "This is some text to be quoted.";
            string result = input.AddQuotes();

            Assert.AreEqual( "'This is some text to be quoted.'", result );
        }

        [TestMethod]
        public void AddQuotes_OnNull()
        {
            string input = null;
            string result = input.AddQuotes();

            Assert.AreEqual( "''", result );
        }

        [TestMethod]
        public void AddQuotes_OnEmpty()
        {
            string input = string.Empty;
            string result = input.AddQuotes();

            Assert.AreEqual( "''", result );
        }

        [TestMethod]
        public void AddDoubleQuotes()
        {
            string input = "This is some text to be quoted.";
            string result = input.AddDoubleQuotes();

            Assert.AreEqual( "\"This is some text to be quoted.\"", result );
        }

        [TestMethod]
        public void AddDoubleQuotes_OnNull()
        {
            string input = null;
            string result = input.AddDoubleQuotes();

            Assert.AreEqual( "\"\"", result );
        }

        [TestMethod]
        public void AddDoubleQuotes_OnEmpty()
        {
            string input = string.Empty;
            string result = input.AddDoubleQuotes();

            Assert.AreEqual( "\"\"", result );
        }

        [TestMethod]
        public void ToBase64()
        {
            string input = "This is some text to be base 64 encoded.";
            string result = input.ToBase64();

            Assert.AreEqual( "VGhpcyBpcyBzb21lIHRleHQgdG8gYmUgYmFzZSA2NCBlbmNvZGVkLg==", result );
        }

        [TestMethod]
        public void ToBase64_OnNull()
        {
            string input = null;
            string result = input.ToBase64();

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void ToBase64_OnEmpty()
        {
            string input = string.Empty;
            string result = input.ToBase64();

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void ToBase64_UrlEncoded()
        {
            string input = "This is some text to be base 64 encoded.";
            string result = input.ToBase64( true );

            Assert.AreEqual( "VGhpcyBpcyBzb21lIHRleHQgdG8gYmUgYmFzZSA2NCBlbmNvZGVkLg%3D%3D", result );
        }

        [TestMethod]
        public void ToBase64_UrlEncodedOnNull()
        {
            string input = null;
            string result = input.ToBase64( true );

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void ToBase64_UrlEncodedOnEmpty()
        {
            string input = string.Empty;
            string result = input.ToBase64( true );

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void FromBase64()
        {
            string input = "VGhpcyBpcyBzb21lIHRleHQgdG8gYmUgYmFzZSA2NCBlbmNvZGVkLg==";
            string result = input.FromBase64();

            Assert.AreEqual( "This is some text to be base 64 encoded.", result );
        }

        [TestMethod]
        public void FromBase64_OnNull()
        {
            string input = null;
            string result = input.FromBase64();

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void FromBase64_OnEmpty()
        {
            string input = string.Empty;
            string result = input.FromBase64();

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void FromBase64_UrlEncoded()
        {
            string input = "VGhpcyBpcyBzb21lIHRleHQgdG8gYmUgYmFzZSA2NCBlbmNvZGVkLg%3d%3d";
            string result = input.FromBase64( true );

            Assert.AreEqual( "This is some text to be base 64 encoded.", result );
        }

        [TestMethod]
        public void FromBase64_UrlEncodedOnNull()
        {
            string input = null;
            string result = input.FromBase64( true );

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void FromBase64_UrlEncodedOnEmpty()
        {
            string input = string.Empty;
            string result = input.FromBase64( true );

            Assert.AreEqual( string.Empty, result );
        }
        
        [TestMethod]
        public void ToByteArray()
        {
            string input = "byte array";
            byte[] result = input.ToByteArray();
            byte[] expected = new byte[] { 98, 0, 121, 0, 116, 0, 101, 0, 32, 0, 97, 0, 114, 0, 114, 0, 97, 0, 121, 0 };

            Assert.AreEqual( expected.Length, result.Length );
            for( int i = 0; i < expected.Length; i++ )
            {
                Assert.AreEqual( expected[ i ], result[ i ] );
            }
        }

        [TestMethod]
        public void ToByteArray_WithEncoding()
        {
            string input = "byte array";
            byte[] result = input.ToByteArray( Encoding.UTF8 );
            byte[] expected = new byte[] { 98, 121, 116, 101, 32, 97, 114, 114, 97, 121 };

            Assert.AreEqual( expected.Length, result.Length );
            for( int i = 0; i < expected.Length; i++ )
            {
                Assert.AreEqual( expected[ i ], result[ i ] );
            }
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void ToByteArray_OnNull()
        {
            string input = null;
            byte[] result = input.ToByteArray();
        }

        [TestMethod]
        public void ToByteArray_OnEmpty()
        {
            string input = string.Empty;
            byte[] result = input.ToByteArray();

            Assert.AreEqual( 0, result.Length );
        }

        [TestMethod]
        public void LimitLength()
        {
            string input = "A string to be length limited.";
            string result = input.LimitLength( 8 );

            Assert.AreEqual( "A string", result );
        }

        [TestMethod]
        public void LimitLength_OnValueShorterThanMaxLength()
        {
            string input = "A string to be length limited.";
            string result = input.LimitLength( 128 );

            Assert.AreEqual( "A string to be length limited.", result );
        }

        [TestMethod]
        public void LimitLength_WithMaxLength0()
        {
            string input = "A string to be length limited.";
            string result = input.LimitLength( 0 );

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void LimitLength_WithMaxLengthMinusOne()
        {
            string input = "A string to be length limited.";
            string result = input.LimitLength( -1 );
        }

        [TestMethod]
        public void LimitLength_OnNull()
        {
            string input = null;
            string result = input.LimitLength( 8 );

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void LimitLength_OnEmpty()
        {
            string input = string.Empty;
            string result = input.LimitLength( 8 );

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void Reverse()
        {
            string input = "A string to be reversed.";
            string result = input.Reverse();

            Assert.AreEqual( ".desrever eb ot gnirts A", result );
        }

        [TestMethod]
        public void Reverse_OnNull()
        {
            string input = null;
            string result = input.Reverse();

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void Reverse_OnEmpty()
        {
            string input = string.Empty;
            string result = input.Reverse();

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void AppendDelimited()
        {
            string target = "Target|String|Value";
            string delimiter = "|";
            string append = "Append";

            string result = target.AppendDelimited( delimiter, append );

            Assert.AreEqual( "Target|String|Value|Append", result );
        }

        [TestMethod]
        public void AppendDelimited_EmptyTarget()
        {
            string target = string.Empty;
            string delimiter = "|";
            string append = "Append";

            string result = target.AppendDelimited( delimiter, append );

            Assert.AreEqual( "|Append", result );
        }

        [TestMethod]
        public void AppendDelimited_EmptyDelimiter()
        {
            string target = "Target|String|Value";
            string delimiter = string.Empty;
            string append = "Append";

            string result = target.AppendDelimited( delimiter, append );

            Assert.AreEqual( "Target|String|ValueAppend", result );
        }

        [TestMethod]
        public void AppendDelimited_EmptyAppend()
        {
            string target = "Target|String|Value";
            string delimiter = "|";
            string append = string.Empty;

            string result = target.AppendDelimited( delimiter, append );

            Assert.AreEqual( "Target|String|Value|", result );
        }

        [TestMethod]
        [ExpectedException( typeof( NullReferenceException ) )]
        public void AppendDelimited_NullTarget()
        {
            string target = null;
            string delimiter = "|";
            string append = "Append";

            string result = target.AppendDelimited( delimiter, append );

            Assert.AreEqual( "|Append", result );
        }

        [TestMethod]
        [ExpectedException( typeof( NullReferenceException ) )]
        public void AppendDelimited_NullDelimiter()
        {
            string target = "Target|String|Value";
            string delimiter = null;
            string append = "Append";

            string result = target.AppendDelimited( delimiter, append );

            Assert.AreEqual( "Target|String|ValueAppend", result );
        }

        [TestMethod]
        [ExpectedException( typeof( NullReferenceException ) )]
        public void AppendDelimited_NullAppend()
        {
            string target = "Target|String|Value";
            string delimiter = "|";
            string append = null;

            string result = target.AppendDelimited( delimiter, append );

            Assert.AreEqual( "Target|String|Value|", result );
        }

        [TestMethod]
        public void Prepend_CountOf1()
        {
            string original = "original";
            string prepended = original.Prepend( '\x0009', 1 );

            Assert.AreEqual( 9, prepended.Count() );
            Assert.AreEqual( "\toriginal", prepended );
        }

        [TestMethod]
        public void Prepend_CountOf0()
        {
            string original = "original";
            string prepended = original.Prepend( '?', 0 );

            Assert.AreEqual( 8, prepended.Count() );
            Assert.AreEqual( "original", prepended );
        }

        [TestMethod]
        public void Prepend_NegativeCount()
        {
            string original = "original";
            string prepended = original.Prepend( '?', -1 );

            Assert.AreEqual( 8, prepended.Count() );
            Assert.AreEqual( "original", prepended );
        }

        [TestMethod]
        public void Prepend_CountOf10()
        {
            string original = "original";
            string prepended = original.Prepend( ' ', 10 );

            Assert.AreEqual( 18, prepended.Count() );
            Assert.AreEqual( "          original", prepended );
        }

        [TestMethod]
        public void Prepend_OnEmptyString()
        {
            string prepended = string.Empty.Prepend( ' ', 10 );

            Assert.AreEqual( 10, prepended.Count() );
            Assert.AreEqual( "          ", prepended );
        }


        [TestMethod]
        public void Append_CountOf1()
        {
            string original = "original";
            string appended = original.Append( '\x0009', 1 );

            Assert.AreEqual( 9, appended.Count() );
            Assert.AreEqual( "original\t", appended );
        }

        [TestMethod]
        public void Append_CountOf0()
        {
            string original = "original";
            string appended = original.Append( '?', 0 );

            Assert.AreEqual( 8, appended.Count() );
            Assert.AreEqual( "original", appended );
        }

        [TestMethod]
        public void Append_NegativeCount()
        {
            string original = "original";
            string appended = original.Append( '?', -1 );

            Assert.AreEqual( 8, appended.Count() );
            Assert.AreEqual( "original", appended );
        }

        [TestMethod]
        public void Append_CountOf10()
        {
            string original = "original";
            string appended = original.Append( ' ', 10 );

            Assert.AreEqual( 18, appended.Count() );
            Assert.AreEqual( "original          ", appended );
        }

        [TestMethod]
        public void Append_OnEmptyString()
        {
            string appended = string.Empty.Append( ' ', 10 );

            Assert.AreEqual( 10, appended.Count() );
            Assert.AreEqual( "          ", appended );
        }

        [TestMethod]
        public void Fill()
        {
            string input = "A string. ";
            string result = string.Empty.Fill( input, 30 );

            Assert.AreEqual( "A string. A string. A string. ", result );
        }

        [TestMethod]
        public void Fill_NonMultipleLength()
        {
            string input = "A string. ";
            string result = string.Empty.Fill( input, 20 );

            Assert.AreEqual( "A string. A string. ", result );
        }

        [TestMethod]
        public void Fill_LengthIsOneMultiple()
        {
            string input = "A string. ";
            string result = string.Empty.Fill( input, 10 );

            Assert.AreEqual( "A string. ", result );
        }

        [TestMethod]
        public void Fill_LengthIsHalfMultiple()
        {
            string input = "A string.";
            string result = string.Empty.Fill( input, 5 );

            Assert.AreEqual( "A str", result );
        }

        [TestMethod]
        public void Fill_OnNull()
        {
            string input = null;
            string result = string.Empty.Fill( input, 5 );

            Assert.IsNull( result );
        }

        [TestMethod]
        public void Fill_OnEmpty()
        {
            string input = string.Empty;
            string result = string.Empty.Fill( input, 5 );

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void Fill_WithLength0()
        {
            string input = "A string.";
            string result = string.Empty.Fill( input, 0 );

            Assert.AreEqual( string.Empty, result );
        }

        [TestMethod]
        public void Fill_WithLengthMinusOne()
        {
            string input = "A string.";
            string result = string.Empty.Fill( input, -1 );

            Assert.AreEqual( string.Empty, result );
        }
    }
}
