using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Common;
using org.ncore.Extensions;

namespace _unittests.org.ncore.Common
{
    /// <summary>
    /// Summary description for HexStringTests
    /// </summary>
    [TestClass]
    public class HexStringTests
    {
        public HexStringTests()
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
        public void FromByteArray()
        {
            byte[] inputByteArray = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128, 255, 127, 63, 31, 15, 7, 3, 1, 0 };
            string result = HexString.FromByteArray( inputByteArray );

            Assert.AreEqual( "0102040810204080FF7F3F1F0F07030100", result );
        }

        [TestMethod]
        public void FromByteArray_WithLength5()
        {
            byte[] inputByteArray = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128, 255, 127, 63, 31, 15, 7, 3, 1, 0 };
            string result = HexString.FromByteArray( inputByteArray, 5 );

            Assert.AreEqual( "0102040810", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void FromByteArray_WithLength0()
        {
            byte[] inputByteArray = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128, 255, 127, 63, 31, 15, 7, 3, 1, 0 };
            string result = HexString.FromByteArray( inputByteArray, 0 );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void FromByteArray_WithLengthMinusOne()
        {
            byte[] inputByteArray = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128, 255, 127, 63, 31, 15, 7, 3, 1, 0 };
            string result = HexString.FromByteArray( inputByteArray, -1 );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void FromByteArray_WithLengthGreaterThanArraySize()
        {
            byte[] inputByteArray = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128, 255, 127, 63, 31, 15, 7, 3, 1, 0 };
            string result = HexString.FromByteArray( inputByteArray, 20 );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void FromByteArray_WithZeroElementArray()
        {
            byte[] inputByteArray = new byte[] { };
            string result = HexString.FromByteArray( inputByteArray );
        }

        [TestMethod]
        public void FromCharArray()
        {
            char[] inputCharArray = new char[] { 'a', 'S', 'd', 'F', 'j', 'K', 'l', ';' };
            string result = HexString.FromCharArray( inputCharArray );

            Assert.AreEqual( "615364466A4B6C3B", result );
        }

        [TestMethod]
        public void FromCharArray_WithLength5()
        {
            char[] inputCharArray = new char[] { 'a', 'S', 'd', 'F', 'j', 'K', 'l', ';' };
            string result = HexString.FromCharArray( inputCharArray, 5 );

            Assert.AreEqual( "615364466A", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void FromCharArray_WithLength0()
        {
            char[] inputCharArray = new char[] { 'a', 'S', 'd', 'F', 'j', 'K', 'l', ';' };
            string result = HexString.FromCharArray( inputCharArray, 0 );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void FromCharArray_WithLengthMinusOne()
        {
            char[] inputCharArray = new char[] { 'a', 'S', 'd', 'F', 'j', 'K', 'l', ';' };
            string result = HexString.FromCharArray( inputCharArray, -1 );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void FromCharArray_WithLengthGreaterThanArraySize()
        {
            char[] inputCharArray = new char[] { 'a', 'S', 'd', 'F', 'j', 'K', 'l', ';' };
            string result = HexString.FromCharArray( inputCharArray, 20 );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void FromCharArray_WithZeroElementArray()
        {
            char[] inputCharArray = new char[] { };
            string result = HexString.FromCharArray( inputCharArray );
        }

        [TestMethod]
        public void ToCharArray()
        {
            string input = "615364466A4B6C3B";
            char[] result = HexString.ToCharArray( input );
            char[] comparison = new char[] { 'a', 'S', 'd', 'F', 'j', 'K', 'l', ';' };

            Assert.IsTrue( comparison.SequenceEqual( result ) );
        }

        [TestMethod]
        public void ToCharArray_OnEmpty()
        {
            string input = string.Empty;
            char[] result = HexString.ToCharArray( input );
            char[] comparison = new char[ 0 ];

            Assert.IsTrue( comparison.SequenceEqual( result ) );
        }

        [TestMethod]
        [ExpectedException( typeof( System.NullReferenceException ) )]
        public void ToCharArray_OnNull()
        {
            string input = null;
            char[] result = HexString.ToCharArray( input );
        }

        [TestMethod]
        public void ToByteArray()
        {
            string input = "0102040810204080FF7F3F1F0F07030100";
            byte[] result = HexString.ToByteArray( input );
            byte[] comparison = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128, 255, 127, 63, 31, 15, 7, 3, 1, 0 };

            Assert.IsTrue( comparison.SequenceEqual( result ) );
        }

        [TestMethod]
        public void ToByteArray_OnEmpty()
        {
            string input = string.Empty;
            byte[] result = HexString.ToByteArray( input );
            byte[] comparison = new byte[ 0 ];

            Assert.IsTrue( comparison.SequenceEqual( result ) );
        }

        [TestMethod]
        [ExpectedException( typeof( System.NullReferenceException ) )]
        public void ToByteArray_OnNull()
        {
            string input = null;
            byte[] result = HexString.ToByteArray( input );
        }
    }
}
