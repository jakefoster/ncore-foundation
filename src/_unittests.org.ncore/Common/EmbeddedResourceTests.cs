using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using org.ncore.Common;

namespace _unittests.org.ncore.Common
{
    /// <summary>
    /// Summary description for EmbeddedResourceTests
    /// </summary>
    [TestClass]
    public class EmbeddedResourceTests
    {
        public EmbeddedResourceTests()
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

        // NOTE: Tests the basic functionality passing in the fully qualified resource name and assembly reference.  JF
        #region Load...() qualified tests
        [TestMethod]
        public void LoadAsStream()
        {
            Stream resource = EmbeddedResource.LoadAsStream( "_unittests.org.ncore.Resources.EmbeddedTextFile.txt", this.GetType().Assembly );

            Assert.IsNotNull( resource );
        }

        [TestMethod]
        [ExpectedException( typeof( MissingResourceException ) )]
        public void LoadAsStream_ThrowsMissingResourceException()
        {
            Stream resource = EmbeddedResource.LoadAsStream( "_unittests.org.ncore.Resources.NotAResource", this.GetType().Assembly );
        }

        [TestMethod]
        public void LoadAsString()
        {
            string resource = EmbeddedResource.LoadAsString( "_unittests.org.ncore.Resources.EmbeddedTextFile.txt", this.GetType().Assembly );
            Assert.IsNotNull( resource );
            Assert.AreEqual( "This is an embedded text file.", resource );
        }

        [TestMethod]
        public void LoadAsXmlDocument()
        {
            XmlDocument resource = EmbeddedResource.LoadAsXmlDocument( "_unittests.org.ncore.Resources.EmbeddedXmlFile.xml", this.GetType().Assembly );
            Assert.IsNotNull( resource );
            string elementNodeText = resource.SelectSingleNode( "/Test/ElementNode" ).InnerText;
            Assert.AreEqual( "This is a test xml element node.", elementNodeText );
        }

        [TestMethod]
        public void LoadAsXDocument()
        {
            XDocument resource = EmbeddedResource.LoadAsXDocument( "_unittests.org.ncore.Resources.EmbeddedXmlFile.xml", this.GetType().Assembly );
            Assert.IsNotNull( resource );
            string elementText = resource.Element( "Test" ).Element( "ElementNode" ).Value;
            Assert.AreEqual( "This is a test xml element node.", elementText );
        }

        [TestMethod]
        public void LoadAsBitmap()
        {
            Bitmap resource = EmbeddedResource.LoadAsBitmap( "_unittests.org.ncore.Resources.EmbeddedBitmap.bmp", this.GetType().Assembly );
            Assert.IsNotNull( resource );
        }

        [TestMethod]
        [ExpectedException( typeof( System.ArgumentException ) )]
        public void LoadAsBitmap_BadCast()
        {
            Bitmap resource = EmbeddedResource.LoadAsBitmap( "_unittests.org.ncore.Resources.EmbeddedTextFile.txt", this.GetType().Assembly );
        }

        [TestMethod]
        public void LoadAsIcon()
        {
            Icon resource = EmbeddedResource.LoadAsIcon( "_unittests.org.ncore.Resources.EmbeddedIcon.ico", this.GetType().Assembly );
            Assert.IsNotNull( resource );
        }

        [TestMethod]
        [ExpectedException( typeof( System.ArgumentException ) )]
        public void LoadAsIcon_ThrowsArgumentException()
        {
            Icon resource = EmbeddedResource.LoadAsIcon( "_unittests.org.ncore.Resources.EmbeddedTextFile.txt", this.GetType().Assembly );
        }
        #endregion

        // NOTE: Tests the convenience loader methods that allow "unqualified" resource names to be passed in with no assembly reference.
        //  It's important to note that these methods will never work on an assembly where the assembly name differs from the namespace
        //  or where the resources aren't directly under that "root" namespace.  This is because the namespace path is inferred from 
        //  the assembly name, and the path to the resources is always assumed to be directly under this namespace.  In cases where either
        //  of these assumptions are incorrect callers should use the "fully qualified" versions of these methods so that correct assembly 
        //  name and the full path to the resources can be manually specified.  JF
        #region Load...() unqualified tests
        [TestMethod]
        public void LoadAsStreamUnqualified()
        {
            Stream resource = EmbeddedResource.LoadAsStream( "Resources.EmbeddedTextFile.txt" );

            Assert.IsNotNull( resource );
        }

        [TestMethod]
        [ExpectedException( typeof( MissingResourceException ) )]
        public void LoadAsStreamUnqualified_ThrowsMissingResourceException()
        {
            Stream resource = EmbeddedResource.LoadAsStream( "Resources.NotAResource" );
        }

        [TestMethod]
        public void LoadAsStringUnqualified()
        {
            string resource = EmbeddedResource.LoadAsString( "Resources.EmbeddedTextFile.txt" );
            Assert.IsNotNull( resource );
            Assert.AreEqual( "This is an embedded text file.", resource );
        }

        [TestMethod]
        public void LoadAsXmlDocumentUnqualified()
        {
            XmlDocument resource = EmbeddedResource.LoadAsXmlDocument( "Resources.EmbeddedXmlFile.xml" );
            Assert.IsNotNull( resource );
            string elementNodeText = resource.SelectSingleNode( "/Test/ElementNode" ).InnerText;
            Assert.AreEqual( "This is a test xml element node.", elementNodeText );
        }

        [TestMethod]
        public void LoadAsXDocumentUnqualified()
        {
            XDocument resource = EmbeddedResource.LoadAsXDocument( "Resources.EmbeddedXmlFile.xml" );
            Assert.IsNotNull( resource );
            string elementText = resource.Element( "Test" ).Element( "ElementNode" ).Value;
            Assert.AreEqual( "This is a test xml element node.", elementText );
        }

        [TestMethod]
        public void LoadAsBitmapUnqualified()
        {
            Bitmap resource = EmbeddedResource.LoadAsBitmap( "Resources.EmbeddedBitmap.bmp" );
            Assert.IsNotNull( resource );
        }

        [TestMethod]
        [ExpectedException( typeof( System.ArgumentException ) )]
        public void LoadAsBitmapUnqualified_BadCast()
        {
            Bitmap resource = EmbeddedResource.LoadAsBitmap( "Resources.EmbeddedTextFile.txt" );
        }

        [TestMethod]
        public void LoadAsIconUnqualified()
        {
            Icon resource = EmbeddedResource.LoadAsIcon( "Resources.EmbeddedIcon.ico" );
            Assert.IsNotNull( resource );
        }

        [TestMethod]
        [ExpectedException( typeof( System.ArgumentException ) )]
        public void LoadAsIconUnqualified_ThrowsArgumentException()
        {
            Icon resource = EmbeddedResource.LoadAsIcon( "Resources.EmbeddedTextFile.txt" );
        }
        #endregion
    }
}
