// TODO: Need more tests to cover various overloads and edge cases.  JF

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Exceptions;

namespace _unittests.org.ncore.Exceptions
{
    /// <summary>
    /// Summary description for RuntimeConditionTests
    /// </summary>
    [TestClass]
    public class RuntimeConditionTests
    {
        public RuntimeConditionTests()
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
        public void BasicThrow()
        {
            try
            {
                throw new RuntimeCondition();
            }
            catch( ApplicationException exception )
            {
                Assert.IsTrue( exception is BaseException );
                Assert.IsTrue( exception is RuntimeCondition );
                RuntimeCondition runtimeCondition = exception as RuntimeCondition;
                Assert.AreEqual( RuntimeCondition.DEFAULT_MESSAGE, runtimeCondition.Message );
                Assert.AreEqual( RuntimeCondition.DEFAULT_INSTRUCTIONS, runtimeCondition.Instructions );
            }
        }

        [TestMethod]
        public void Throw_WithMessage()
        {
            try
            {
                throw new RuntimeCondition( "Not the default message." );
            }
            catch( ApplicationException exception )
            {
                Assert.IsTrue( exception is BaseException );
                Assert.IsTrue( exception is RuntimeCondition );
                RuntimeCondition runtimeCondition = exception as RuntimeCondition;
                Assert.AreEqual( "Not the default message.", runtimeCondition.Message );
                Assert.AreEqual( RuntimeCondition.DEFAULT_INSTRUCTIONS, runtimeCondition.Instructions );
            }
        }

        [TestMethod]
        public void Throw_WithMessageAndInstructionText()
        {
            try
            {
                throw new RuntimeCondition( "Not the default message.", "Not the default instruction expression." );
            }
            catch( ApplicationException exception )
            {
                Assert.IsTrue( exception is BaseException );
                Assert.IsTrue( exception is RuntimeCondition );
                RuntimeCondition runtimeCondition = exception as RuntimeCondition;
                Assert.AreEqual( "Not the default message.", runtimeCondition.Message );
                Assert.AreEqual( "Not the default instruction expression.", runtimeCondition.Instructions );
            }
        }

        [TestMethod]
        public void Throw_WithInnerException()
        {
            ArgumentException innerException = new ArgumentException( "The parameter cannot be null.", "fubar" );
            try
            {
                throw new RuntimeCondition( innerException );
            }
            catch( ApplicationException exception )
            {
                Assert.IsTrue( exception is BaseException );
                Assert.IsTrue( exception is RuntimeCondition );
                RuntimeCondition runtimeCondition = exception as RuntimeCondition;
                Assert.AreEqual( innerException, runtimeCondition.InnerException );
                Assert.AreEqual( RuntimeCondition.DEFAULT_MESSAGE, runtimeCondition.Message );
                Assert.AreEqual( RuntimeCondition.DEFAULT_INSTRUCTIONS, runtimeCondition.Instructions );
            }
        }
    }
}
