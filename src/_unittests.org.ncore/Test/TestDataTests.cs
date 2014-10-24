using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Common;
using org.ncore.Diagnostics;
using org.ncore.Test;

namespace _unittests.org.ncore.Test
{
    /// <summary>
    /// Summary description for TestDataTests
    /// </summary>
    [TestClass]
    public class TestDataTests
    {
        private static Regex _isValidEmailPattern = new Regex(
            @"^[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$",
            RegexOptions.IgnoreCase
             | RegexOptions.Multiline
             | RegexOptions.IgnorePatternWhitespace
             | RegexOptions.Compiled );

        private string[] _lastNames;
        private string[] _femaleNames;
        private string[] _maleNames;

        public TestDataTests()
        {
            string raw = EmbeddedResource.LoadAsString( 
                "org.ncore.Resources.Common.LastNames.txt", Assembly.GetAssembly( typeof(TestData) ) );
            _lastNames = raw.Split( new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries );

            raw = EmbeddedResource.LoadAsString(
                "org.ncore.Resources.Common.FemaleNames.txt", Assembly.GetAssembly( typeof( TestData ) ) );
            _femaleNames = raw.Split( new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries );

            raw = EmbeddedResource.LoadAsString(
                "org.ncore.Resources.Common.MaleNames.txt", Assembly.GetAssembly( typeof( TestData ) ) );
            _maleNames = raw.Split( new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries );
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
        public void TestEmailStringTestKey()
        {
            string emailString = TestData.GenerateEmailAddress();

            Assert.IsTrue( emailString.Length > 0 );
            Assert.IsTrue( _isValidEmailPattern.IsMatch( emailString ) );
        }

        [TestMethod]
        public void TestCreateTestKey_TestKey0()
        {
            string testKey = TestData.CreateTestKey( 0 );
            Assert.IsTrue( testKey.Length > 0 );
        }

        [TestMethod]
        public void TestCreateTestKey_TestKey35()
        {
            string testKey = TestData.CreateTestKey( 35 );
            Assert.IsTrue( testKey.Length == 35 );
        }

        [TestMethod]
        public void TestCreateTestKey_TestKey1000()
        {
            string testKey = TestData.CreateTestKey( 1000 );
            Assert.AreEqual( 1000, testKey.Length );
        }

        [TestMethod]
        public void TestCreateTestKey_TestKey1025()
        {
            string testKey = TestData.CreateTestKey( 1025 );
            Assert.AreEqual( 1024, testKey.Length );
        }

        [TestMethod]
        public void GenerateLastName()
        {
            string name = TestData.GenerateLastName();

            Assert.AreNotEqual( string.Empty, name );
            Assert.IsTrue( _lastNames.Contains( name ) );
        }

        [TestMethod]
        public void GenerateFemaleName()
        {
            string name = TestData.GenerateFemaleGivenName();

            Assert.AreNotEqual( string.Empty, name );
            Assert.IsTrue( _femaleNames.Contains( name ) );
        }

        [TestMethod]
        public void GenerateMaleFirstName()
        {
            string name = TestData.GenerateMaleGivenName();

            Assert.AreNotEqual( string.Empty, name );
            Assert.IsTrue( _maleNames.Contains( name ) );
        }

        [TestMethod]
        public void GenerateFemaleNamePrefix()
        {
            string value = TestData.GenerateFemaleNamePrefix();

            Assert.AreNotEqual( string.Empty, value );
            Assert.IsTrue( new TestData.FemaleNamePrefixes().Contains( value ) );
        }

        [TestMethod]
        public void GenerateMaleNamePrefix()
        {
            string value = TestData.GenerateMaleNamePrefix();

            Assert.AreNotEqual( string.Empty, value );
            Assert.IsTrue( new TestData.MaleNamePrefixes().Contains( value ) );
        }


        [TestMethod]
        public void GenerateNameSuffix()
        {
            string value = TestData.GenerateNameSuffix();

            Assert.AreNotEqual( string.Empty, value );
            Assert.IsTrue( new TestData.NameSuffixes().Contains( value ) );
        }


        [TestMethod]
        public void GenerateNameHonorific()
        {
            string value = TestData.GenerateNameHonorific();

            Assert.AreNotEqual( string.Empty, value );
            Assert.IsTrue( new TestData.NameHonorifics().Contains( value ) );
        }

        // TODO: Could probably write a better test here.  JF
        [TestMethod]
        public void GeneratePersonName()
        {
            TestData.PersonName value = TestData.GeneratePersonName();
            Assert.AreNotEqual( string.Empty, value );
        }
    }
}
