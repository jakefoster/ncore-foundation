using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Diagnostics;

namespace _unittests.org.ncore.Diagnostics
{
    // TODO: We should really have some multi-threaded unit tests here since this thing is supposed to be thread-safe and all.  JF

    /// <summary>
    /// Summary description for TextFileWriterTests
    /// </summary>
    [TestClass]
    public class TextFileWriterTests
    {
        public TextFileWriterTests()
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
        public void WriteLine_SingleLine()
        {
            string fileLocation = Path.Combine( Environment.CurrentDirectory, "test.txt" );
            try
            {
                TextFileWriter writer = new TextFileWriter( fileLocation );
                writer.WriteLine( "Foo" );

                string fileContents = File.ReadAllText( fileLocation );

                Assert.AreEqual( fileContents, "Foo\r\n" );

            }
            finally
            {
                if( File.Exists( fileLocation ) )
                {
                    File.Delete( fileLocation );
                }

            }
        }

        [TestMethod]
        public void WriteLine_MultipleLines()
        {
            string fileLocation = Path.Combine( Environment.CurrentDirectory, "test.txt" );
            try
            {
                TextFileWriter writer = new TextFileWriter( fileLocation );
                writer.WriteLine( "Foo" );
                writer.WriteLine( "Bar" );
                writer.WriteLine( "Fubar" );

                string fileContents = File.ReadAllText( fileLocation );

                Assert.AreEqual( fileContents, "Foo\r\nBar\r\nFubar\r\n" );

            }
            finally
            {
                if( File.Exists( fileLocation ) )
                {
                    File.Delete( fileLocation );
                }

            }
        }

        [TestMethod]
        public void Write()
        {
            string fileLocation = Path.Combine( Environment.CurrentDirectory, "test.txt" );
            try
            {
                TextFileWriter writer = new TextFileWriter( fileLocation );
                writer.Write( "Foo" );
                writer.Write( "Bar" );
                writer.Write( "Fubar" );

                string fileContents = File.ReadAllText( fileLocation );

                Assert.AreEqual( fileContents, "FooBarFubar" );

            }
            finally
            {
                if( File.Exists( fileLocation ) )
                {
                    File.Delete( fileLocation );
                }

            }
        }

        [TestMethod]
        public void Write_MultipleLines()
        {
            string fileLocation = Path.Combine( Environment.CurrentDirectory, "test.txt" );
            try
            {
                TextFileWriter writer = new TextFileWriter( fileLocation );
                writer.Write( "Foo\r\n" );
                writer.Write( "Bar\r\n" );
                writer.Write( "Fubar\r\n" );

                string fileContents = File.ReadAllText( fileLocation );

                Assert.AreEqual( fileContents, "Foo\r\nBar\r\nFubar\r\n" );

            }
            finally
            {
                if( File.Exists( fileLocation ) )
                {
                    File.Delete( fileLocation );
                }

            }
        }


        [TestMethod]
        public void WriteLine_WithSharedWriteAccess()
        {
            string fileLocation = Path.Combine( Environment.CurrentDirectory, "test.txt" );
            try
            {
                TextFileWriter writer = new TextFileWriter( fileLocation )
                                            {
                                                SharedWriteAccess = false,
                                            };
                writer.WriteLine( "Foo" );

                string fileContents = File.ReadAllText( fileLocation );

                Assert.AreEqual( fileContents, "Foo\r\n" );

            }
            finally
            {
                if( File.Exists( fileLocation ) )
                {
                    File.Delete( fileLocation );
                }

            }
        }

    }
}
