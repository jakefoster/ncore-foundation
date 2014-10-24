// TODO: Need more unit tests.  JF

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Extensions;

namespace _unittests.org.ncore.Extensions
{
    /// <summary>
    /// Summary description for BoolArray
    /// </summary>
    [TestClass]
    public class System_BoolArrayTests
    {
        public System_BoolArrayTests()
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

        // TODO: Need a ton more tests.  JF

        [TestMethod]
        public void ToUInt32_0()
        {
            bool[] bits = new bool[32];
            UInt32 result = bits.ToUInt32();

            Assert.AreEqual( (UInt32)0, result );
        }

        [TestMethod]
        public void ToUInt32_41()
        {
            bool[] bits = new bool[ 32 ];
            bits.SetBit( 1, true );
            bits.SetBit( 8, true );
            bits.SetBit( 32, true );

            UInt32 result = bits.ToUInt32();

            Assert.AreEqual( (UInt32)41, result );
        }

        [TestMethod]
        public void ToByte()
        {
            bool[] bits = new bool[ 8 ];
            bits.SetBit( 1, true );
            bits.SetBit( 2, true );
            bits.SetBit( 4, true );
            bits.SetBit( 8, true );
            bits.SetBit( 16, true );
            bits.SetBit( 32, true );
            bits.SetBit( 64, true );
            bits.SetBit( 128, true );

            Byte result = bits.ToByte();

            Assert.AreEqual( (byte)255, result );
        }

        [TestMethod]
        public void Fill_False()
        {
            bool[] bits = new bool[ 8 ].Fill( false );
            bool[] expected = new bool[ 8 ];

            Assert.IsTrue( expected.SequenceEqual( bits ) );
        }

        [TestMethod]
        public void Fill_True()
        {
            bool[] bits = new bool[ 8 ].Fill( true );
            bool[] expected = new bool[ 8 ];
            expected.SetBit( 1, true );
            expected.SetBit( 2, true );
            expected.SetBit( 4, true );
            expected.SetBit( 8, true );
            expected.SetBit( 16, true );
            expected.SetBit( 32, true );
            expected.SetBit( 64, true );
            expected.SetBit( 128, true );

            Assert.IsTrue( expected.SequenceEqual( bits ) );
        }

        [TestMethod]
        public void ToText()
        {
            bool[] bits = new bool[ 8 ];
            bits.SetBit( 1, true );
            bits.SetBit( 4, true );
            bits.SetBit( 16, true );
            bits.SetBit( 64, true );

            string text = bits.ToText();

            Assert.AreEqual( "01010101", text );
        }

        [TestMethod]
        public void ToText_BigEndian()
        {
            bool[] bits = new bool[ 8 ];
            bits.SetBit( 1, true );
            bits.SetBit( 4, true );
            bits.SetBit( 16, true );
            bits.SetBit( 64, true );

            string text = bits.ToText( false );

            Assert.AreEqual( "10101010", text );
        }

        // TODO: Seems kind of silly to have this test.  SetBits is used in all the other tests.
        //  Or, vice-versa.  So really it's already been tested.  Eliminate?  JF
        [TestMethod]
        public void SetBits()
        {
            bool[] bits = new bool[ 8 ];
            bits.SetBit( 1, true );
            bits.SetBit( 4, true );
            bits.SetBit( 16, true );
            bits.SetBit( 64, true );

            string text = bits.ToText( false );

            Assert.AreEqual( "10101010", text );
        }
    }
}
