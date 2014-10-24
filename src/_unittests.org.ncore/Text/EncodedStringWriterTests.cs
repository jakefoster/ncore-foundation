using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using org.ncore.Diagnostics;
using org.ncore.Text;
using org.ncore.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _unittests.org.ncore.Text
{
    /// <summary>
    /// Summary description for EncodedStringWriterTests
    /// </summary>
    [TestClass]
    public class EncodedStringWriterTests
    {
        public EncodedStringWriterTests()
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
        public void SimpleAscii()
        {
            Encoding encoding = Encoding.ASCII;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "A" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 1, bytes.Length );
            Assert.IsTrue( new byte[] { 65 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\u0041", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SimpleUtf7()
        {
            Encoding encoding = Encoding.UTF7;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "A" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 1, bytes.Length );
            Assert.IsTrue( new byte[] { 65 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\u0041", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SimpleUtf8()
        {
            Encoding encoding = Encoding.UTF8;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "A" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 1, bytes.Length );
            Assert.IsTrue( new byte[] { 65 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\u0041", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SimpleUtf16()
        {
            Encoding encoding = Encoding.Unicode;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "A" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 2, bytes.Length );
            Assert.IsTrue( new byte[] { 65, 0 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\u0041", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SimpleUtf16BigEndian()
        {
            Encoding encoding = Encoding.GetEncoding( "utf-16be" );
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "A" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 2, bytes.Length );
            Assert.IsTrue( new byte[] { 0, 65 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\u0041", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SimpleUtf32()
        {
            Encoding encoding = Encoding.UTF32;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "A" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 4, bytes.Length );
            Assert.IsTrue( new byte[]{ 65, 0, 0, 0 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\u0041", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SimpleUtf32BigEndian()
        {
            Encoding encoding = Encoding.GetEncoding( "utf-32be" );
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "A" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 4, bytes.Length );
            Assert.IsTrue( new byte[]{ 0, 0, 0, 65 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\u0041", bytes.ToText( encoding ) );
        }

        [TestMethod]
        // NOTE: Automatically turns any non-supported (e.g. non-ascii) value into "?" or "??" character.  JF
        public void SIP_28FEA_Ascii_PerformsImplicitConversion()
        {
            Encoding encoding = Encoding.ASCII;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "𨿪" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 2, bytes.Length );
            Assert.IsTrue( new byte[]{ 63, 63 }.SequenceEqual( bytes ) );
        }

        [TestMethod]
        public void BMP_0F36_Utf8()
        {
            Encoding encoding = Encoding.UTF8;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "༶" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 3, bytes.Length );
            Assert.IsTrue( new byte[]{ 224, 188, 182 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\u0F36", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void BMP_0F36_Utf16()
        {
            Encoding encoding = Encoding.Unicode;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "༶" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 2, bytes.Length );
            Assert.IsTrue( new byte[] { 54, 15 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\u0F36", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void BMP_0F36_Utf32()
        {
            Encoding encoding = Encoding.UTF32;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "༶" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 4, bytes.Length );
            Assert.IsTrue( new byte[] { 54, 15, 0, 0 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\u0F36", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SMP_1D33F_Utf8()
        {
            Encoding encoding = Encoding.UTF8;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "𝌿" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 4, bytes.Length );
            Assert.IsTrue( new byte[] { 240, 157, 140, 191 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\U0001D33F", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SMP_1D33F_Utf16()
        {
            Encoding encoding = Encoding.Unicode;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "𝌿" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 4, bytes.Length );
            Assert.IsTrue( new byte[] { 52, 216, 63, 223 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\U0001D33F", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SMP_1D33F_Utf32()
        {
            Encoding encoding = Encoding.UTF32;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "𝌿" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 4, bytes.Length );
            Assert.IsTrue( new byte[] { 63, 211, 1, 0 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\U0001D33F", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SIP_28FEA_Utf8()
        {
            Encoding encoding = Encoding.UTF8;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "𨿪" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 4, bytes.Length );
            Assert.IsTrue( new byte[] { 240, 168, 191, 170 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\U00028FEA", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SIP_28FEA_Utf16()
        {
            Encoding encoding = Encoding.Unicode;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "𨿪" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 4, bytes.Length );
            Assert.IsTrue( new byte[] { 99, 216, 234, 223 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\U00028FEA", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void SIP_28FEA_Utf32()
        {
            Encoding encoding = Encoding.UTF32;
            EncodedStringWriter writer = new EncodedStringWriter( encoding );
            writer.Write( "𨿪" );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( 4, bytes.Length );
            Assert.IsTrue( new byte[] { 234, 143, 2, 0 }.SequenceEqual( bytes ) );
            Assert.AreEqual( "\U00028FEA", bytes.ToText( encoding ) );
        }

        [TestMethod]
        public void PassIFormatProviderToConstructor()
        {
            Encoding encoding = Encoding.Unicode;
            EncodedStringWriter writer = new EncodedStringWriter( new CultureInfo("ar-DZ"), encoding );
            DateTime dateTime = DateTime.Parse( "5/2/2011 10:20:35 PM" );
            writer.Write( dateTime );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( "02-05-2011 22:20:35", bytes.ToText( encoding ) );
        }


        [TestMethod]
        public void PassStringBuilderToConstructor()
        {
            StringBuilder stringBuilder = new StringBuilder();

            Encoding encoding = Encoding.Unicode;
            EncodedStringWriter writer = new EncodedStringWriter( stringBuilder, encoding );
            writer.Write( "My own string builder." );

            byte[] bytes = encoding.GetBytes( writer.ToString() );

            Assert.AreEqual( "My own string builder.", bytes.ToText( encoding ) );
            Assert.AreSame( stringBuilder, writer.GetStringBuilder() );
            Assert.AreEqual( 22, stringBuilder.Length );
            Assert.AreEqual( "My own string builder.", stringBuilder.ToString() );
        }
    }
}
