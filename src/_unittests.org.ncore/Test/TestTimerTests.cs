using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Test;
using org.ncore.Diagnostics;

namespace _unittests.org.ncore.Test
{
    /// <summary>
    /// Summary description for TestTimerTests
    /// </summary>
    [TestClass]
    public class TestTimerTests
    {
        public TestTimerTests()
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


        // TODO: Purely expository "test" code.  Need real unit tests here.  JF
        [TestMethod]
        public void TestTimer_General()
        {
            Spy.Mark();
            using( TestTimer parentTimer = TestTimer.New() )
            {
                using( TestTimer sleep1 = TestTimer.New( "Sleep1" ) )
                {
                    int sleep = 500;
                    Spy.Trap( () => sleep );
                    System.Threading.Thread.Sleep( sleep );
                }

                using( TestTimer sleep2 = TestTimer.New( "Sleep2" ) )
                {
                    int sleep = 500;
                    Spy.Trap( () => sleep );
                    System.Threading.Thread.Sleep( sleep );
                }
            }
            Spy.Trace( "Done" );
        }
    }
}
