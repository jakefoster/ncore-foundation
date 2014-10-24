// TODO: Need more tests.  JF

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Text;
using org.ncore.Xml;

namespace _unittests.org.ncore.Xml
{
    /// <summary>
    /// Summary description for XSerializerTests
    /// </summary>
    [TestClass]
    public class XSerializerTests
    {
        public XSerializerTests()
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
        public void SerializeToString()
        {
            Fubar original = new Fubar() {Name = "MyFubar", Number = 1};
            original.SetSecretSwitch( true );

            string serialized = XSerializer.SerializeToString( original );

            Assert.AreEqual( "<?xml version=\"1.0\" encoding=\"utf-8\"?><Fubar xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Number>1</Number><Name>MyFubar</Name></Fubar>", serialized );
        }

        [TestMethod]
        public void SerializeToXDocument()
        {
            Fubar original = new Fubar() { Name = "MyFubar", Number = 1 };
            original.SetSecretSwitch( true );

            XDocument serialized = XSerializer.SerializeToXDocument( original );

            Assert.AreEqual( "<Fubar xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Number>1</Number><Name>MyFubar</Name></Fubar>", serialized.ToString( SaveOptions.DisableFormatting ) );
        }

        [TestMethod]
        public void SerializeToXmlDocument()
        {
            Fubar original = new Fubar() { Name = "MyFubar", Number = 1 };
            original.SetSecretSwitch( true );

            XmlDocument serialized = XSerializer.SerializeToXmlDocument( original );

            EncodedStringWriter writer = new EncodedStringWriter( Encoding.UTF8 );
            XmlTextWriter xmlWriter = new XmlTextWriter( writer );
            serialized.WriteTo( xmlWriter );

            Assert.AreEqual( "<?xml version=\"1.0\" encoding=\"utf-8\"?><Fubar xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Number>1</Number><Name>MyFubar</Name></Fubar>", writer.ToString() );
        }

        [TestMethod]
        public void DeserializeFromString()
        {
            Fubar deserialized = (Fubar)XSerializer.Deserialize( "<?xml version=\"1.0\" encoding=\"utf-8\"?><Fubar xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Number>1</Number><Name>MyFubar</Name></Fubar>", typeof( Fubar ) );

            Assert.IsNotNull( deserialized );
            Assert.AreEqual( "MyFubar", deserialized.Name );
            Assert.AreEqual( 1, deserialized.Number );
        }

        [TestMethod]
        public void DeserializeFromXDocument()
        {
            XDocument xml = XDocument.Parse( "<?xml version=\"1.0\" encoding=\"utf-8\"?><Fubar xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Number>1</Number><Name>MyFubar</Name></Fubar>" );
            Fubar deserialized = (Fubar)XSerializer.Deserialize( xml, typeof( Fubar ) );

            Assert.IsNotNull( deserialized );
            Assert.AreEqual( "MyFubar", deserialized.Name );
            Assert.AreEqual( 1, deserialized.Number );
        }

        [TestMethod]
        public void DeserializeFromXmlDocument()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml( "<?xml version=\"1.0\" encoding=\"utf-8\"?><Fubar xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Number>1</Number><Name>MyFubar</Name></Fubar>" );

            Fubar deserialized = (Fubar)XSerializer.Deserialize( xml, typeof( Fubar ) );

            Assert.IsNotNull( deserialized );
            Assert.AreEqual( "MyFubar", deserialized.Name );
            Assert.AreEqual( 1, deserialized.Number );
        }
    }

    public class Fubar
    {
        private bool _secretSwitch;
        public int Number;
        public string Name { get; set; }
        public void SetSecretSwitch( bool value )
        {
            _secretSwitch = value;
        }
    }
}
