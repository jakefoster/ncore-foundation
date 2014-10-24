using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Diagnostics;
using org.ncore.Exceptions;
using org.ncore.Extensions;

namespace _unittests.org.ncore.Extensions
{
    /// <summary>
    /// Summary description for Exception
    /// </summary>
    [TestClass]
    public class System_Exception
    {
        public System_Exception()
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

        // TODO: Really need more tests here.  JF

        [TestMethod]
        public void ToXDocument()
        {
            // ARRANGE
            ApplicationException innerException = new ApplicationException( "The underlying bad thing that happened." );
            Exception exception = null;
            try
            {
                throw new RuntimeException( "Something bad happened and here are the details...", "Something bad happened.", innerException );
            }
            catch( Exception ex )
            {
                exception = ex;
            }

            Dictionary<string,string> interestingData = new Dictionary<string, string>();
            interestingData.Add( "Interesting Value A", "a value" );
            interestingData.Add( "Interesting Value B", "b value" );

            // ACT
            XDocument document = exception.ToXDocument( "SomeUniqueKey", "SomeDescriptionOfTheOperation", interestingData );

            // ASSERT
            Assert.IsNotNull( document );

            XNamespace ns = "http://schemas.ncore.org/Global/Exception/1/";
            Assert.AreEqual( ns + "ExceptionTree", document.Root.Name );
            Assert.AreEqual( "SomeUniqueKey", document.Root.Attribute( "Key" ).Value );
            Assert.AreEqual( "SomeDescriptionOfTheOperation", document.Root.Attribute( "Action" ).Value );
            Assert.AreEqual( "1", document.Root.Attribute( "Revision" ).Value );

            XElement exceptionElement = document.Root.Element( ns + "Exception" );
            Assert.IsNotNull( exceptionElement );
            Assert.AreEqual( "org.ncore.Exceptions.RuntimeException", exceptionElement.Element( ns + "Type" ).Value );
            Assert.AreEqual( "Something bad happened and here are the details...", exceptionElement.Element( ns + "Message" ).Value );
            Assert.AreEqual( "Something bad happened.", exceptionElement.Element( ns + "Instructions" ).Value );
            Assert.AreEqual( "_unittests.org.ncore", exceptionElement.Element( ns + "Source" ).Value );
            Assert.AreEqual( "Void ToXDocument()", exceptionElement.Element( ns + "Operation" ).Value );
            // TODO: How to write so this works on any dev environment?  Note the full path in the StackTrace.
            //  Maybe a regex so we can wildcard that bit (and the line number) in the string matching?  JF
            //Assert.IsTrue( exceptionElement.Element( ns + "StackTrace" ).Value.StartsWith( @"   at _unittests.org.ncore.Extensions.System_Exception.ToXDocument() in C:\dev\cbx\Foundation\src\_unittests.org.ncore\Extensions\System\ExceptionTests.cs:line " ) );

            XElement innerExceptionElement = exceptionElement.Element( ns + "InnerException" ).Element( ns + "Exception" );
            Assert.IsNotNull( exceptionElement );
            Assert.AreEqual( "System.ApplicationException", innerExceptionElement.Element( ns + "Type" ).Value );
            Assert.AreEqual( "The underlying bad thing that happened.", innerExceptionElement.Element( ns + "Message" ).Value );
            Assert.AreEqual( string.Empty, innerExceptionElement.Element( ns + "Instructions" ).Value );
            Assert.AreEqual( string.Empty, innerExceptionElement.Element( ns + "Source" ).Value );
            Assert.AreEqual( string.Empty, innerExceptionElement.Element( ns + "Operation" ).Value );
            Assert.AreEqual( string.Empty, innerExceptionElement.Element( ns + "StackTrace" ).Value );
            Assert.IsTrue( innerExceptionElement.Element( ns + "InnerException" ).IsEmpty );

            XElement metadataElement = document.Root.Element( ns + "Metadata" );
            Assert.AreEqual( 2, metadataElement.Elements().Count() );

            foreach( XElement element in metadataElement.Elements() )
            {
                Assert.AreEqual( ns + "Item", element.Name );
            }

            Assert.AreEqual( "Interesting Value A", metadataElement.Elements().ElementAt( 0 ).Attribute( "Type" ).Value);
            Assert.AreEqual( "a value", metadataElement.Elements().ElementAt( 0 ).Value );

            Assert.AreEqual( "Interesting Value B", metadataElement.Elements().ElementAt( 1 ).Attribute( "Type" ).Value );
            Assert.AreEqual( "b value", metadataElement.Elements().ElementAt( 1 ).Value );

            /*
            <ExceptionTree Key="SomeUniqueKey" Action="SomeDescriptionOfTheOperation" Revision="1" xmlns="http://schemas.ncore.org/Global/Exception/1/">
              <Exception>
                <Type>org.ncore.Exceptions.RuntimeException</Type>
                <Message>Something bad happened and here are the details...</Message>
                <Instructions>Something bad happened.</Instructions>
                <Source>_unittests.org.ncore</Source>
                <Operation>Void ToXDocument()</Operation>
                <StackTrace>   at _unittests.org.ncore.Extensions.System_Exception.ToXDocument() in C:\dev\cbx\org.ncore\src\_unittests.org.ncore\Extensions\System\ExceptionTests.cs:line 74</StackTrace>
                <InnerException>
                  <Exception>
                    <Type>System.ApplicationException</Type>
                    <Message>The underlying bad thing that happened.</Message>
                    <Instructions></Instructions>
                    <Source></Source>
                    <Operation></Operation>
                    <StackTrace></StackTrace>
                    <InnerException />
                  </Exception>
                </InnerException>
              </Exception>
              <Metadata>
                <Item Type="Interesting Value A">a value</Item>
                <Item Type="Interesting Value B">b value</Item>
              </Metadata>
            </ExceptionTree>
            */
        }
    }
}
