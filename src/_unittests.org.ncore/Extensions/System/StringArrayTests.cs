using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Extensions;

namespace _unittests.org.ncore.Extensions
{
    /// <summary>
    /// Summary description for StringArrayTests
    /// </summary>
    [TestClass]
    public class System_StringArrayTests
    {
        public System_StringArrayTests()
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
        public void Tokenize()
        {
            String[] source = new String[]{ "one", "two", "three" };
            string tokenized = source.Tokenize( "|", "'" );

            Assert.AreEqual( "'one'|'two'|'three'", tokenized );
        }

        [TestMethod]
        public void Tokenize_StringEmptyEncapsulator()
        {
            String[] source = new String[] { "one", "two", "three" };
            string tokenized = source.Tokenize( "|", string.Empty );

            Assert.AreEqual( "one|two|three", tokenized );
        }

        [TestMethod]
        public void Tokenize_NullEncapsulator()
        {
            String[] source = new String[] { "one", "two", "three" };
            string tokenized = source.Tokenize( "|", null );

            Assert.AreEqual( "one|two|three", tokenized );
        }

        [TestMethod]
        public void Tokenize_StringEmptyDelimiter()
        {
            String[] source = new String[] { "one", "two", "three" };
            string tokenized = source.Tokenize( string.Empty, "'" );

            Assert.AreEqual( "'one''two''three'", tokenized );
        }

        [TestMethod]
        public void Tokenize_NullDelimiter()
        {
            String[] source = new String[] { "one", "two", "three" };
            string tokenized = source.Tokenize( null, "'" );

            Assert.AreEqual( "'one''two''three'", tokenized );
        }

        [TestMethod]
        public void Tokenize_EmptyStringArray()
        {
            String[] source = new String[]{};
            string tokenized = source.Tokenize( "|", "'" );

            Assert.AreEqual( string.Empty, tokenized );
        }

        [TestMethod]
        public void Tokenize_NullStringArray()
        {
            String[] source = null;
            string tokenized = source.Tokenize( "|", "'" );

            Assert.AreEqual( string.Empty, tokenized );
        }

        [TestMethod]
        public void Tokenize_SingleElementArray()
        {
            String[] source = new String[] { "one" };
            string tokenized = source.Tokenize( "|", "'" );

            Assert.AreEqual( "'one'", tokenized );
        }

        // TODO: Should we have a string.Detokenize()::string[] extension method as well to reverse it?!
    }
}
