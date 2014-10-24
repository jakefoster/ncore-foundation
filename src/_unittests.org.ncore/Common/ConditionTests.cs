using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Common;

namespace _unittests.org.ncore.Common
{
    /// <summary>
    /// Summary description for ConditionTests
    /// </summary>
    [TestClass]
    public class ConditionTests
    {
        public ConditionTests()
        {
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

        #region Assert() tests
        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void Assert_Fail()
        {
            Condition.Assert( true == false );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void Assert_FailWithMessage()
        {
            Condition.Assert( true == false, "true == false" );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void Assert_FailWithSuppliedException()
        {
            ArgumentException exception = new ArgumentException( "The supplied argument was incorrect.", "true == false" );

            Condition.Assert( true == false, exception );
        }

        [TestMethod]
        public void Assert_Succeed()
        {
            Condition.Assert( true == true );
        }

        [TestMethod]
        public void Assert_SucceedWithMessage()
        {
            Condition.Assert( true == true, "true == true" );
        }
        #endregion

        #region Fail() Tests
        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void Fail()
        {
            Condition.Fail();
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void Fail_WithMessage()
        {
            Condition.Fail( "Bad assertion" );
        }
        #endregion

        // NOTE: The basic AssertCaller and AssertNotCaller tests.  JF
        #region AssertCaller() and AssertNotCaller() basic tests
        [TestMethod]
        public void AssertCaller_CallingTypePresent()
        {
            string result = TestAssertCaller( "_unittests.org.ncore.Common.ConditionTests" );

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertCaller_CallingTypeAbsentThrowsAssertionException()
        {
            string result = TestAssertCaller( "" );

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        public void AssertCaller_CallingMethodPresent()
        {
            string result = TestAssertCaller( "_unittests.org.ncore.Common.ConditionTests::AssertCaller_CallingMethodPresent" );

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertCaller_CallingMethodAbsentThrowsAssertionException()
        {
            string result = TestAssertCaller( "" );

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        public void AssertCaller_WildcardOnNamespace()
        {
            string result = TestAssertCaller( "_unittests.org.ncore.Common.*" );

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        public void AssertCaller_WildcardOnClass()
        {
            string result = TestAssertCaller( "_unittests.org.ncore.Common.ConditionTests::*" );

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        public void AssertNotCaller_CallingTypeAbsent()
        {
            string result = TestAssertNotCaller( "" );

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertNotCaller_CallingTypePresentThrowsAssertionException()
        {
            string result = TestAssertNotCaller( "_unittests.org.ncore.Common.ConditionTests" );

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        public void AssertNotCaller_CallingMethodAbsent()
        {
            string result = TestAssertNotCaller( "" );

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertNotCaller_CallingMethodPresentThrowsAssertionException()
        {
            string result = TestAssertNotCaller( "_unittests.org.ncore.Common.ConditionTests::AssertNotCaller_CallingMethodPresentThrowsAssertionException" );

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertNotCaller_WildcardOnNamespaceThrowsAssertionException()
        {
            string result = TestAssertNotCaller( "_unittests.org.ncore.Common.*" );

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertNotCaller_WildcardOnClassThrowsAssertionException()
        {
            string result = TestAssertNotCaller( "_unittests.org.ncore.Common.ConditionTests::*" );

            Assert.AreEqual( "ok", result );
        }

        // NOTE: Helper for basic AssertCaller tests.  JF
        public string TestAssertCaller( string caller )
        {
            Condition.AssertCaller(
                "SomeNamespace.SomeClass::SomeMethod",
                "SomeNamespace.SomeOtherClass",
                caller );

            return "ok";
        }

        // NOTE: Helper for basic AssertNotCaller tests.  JF
        public string TestAssertNotCaller( string caller )
        {
            Condition.AssertNotCaller(
                "SomeNamespace.SomeClass::SomeMethod",
                "SomeNamespace.SomeOtherClass",
                caller );

            return "ok";
        }
        #endregion

        // NOTE: More sophisticated samples of AssertCaller and AssertNotCaller.  Not sure if these really 
        //  clarify all that much.  JF
        #region AssertCaller() and AssertNotCaller() advanced tests
        [TestMethod]
        public void AssertCaller_GoodCaller()
        {
            GoodCaller caller = new GoodCaller();
            string result = caller.EntryPoint();

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertCaller_BadCallerThrowsAssertionException()
        {
            BadCaller caller = new BadCaller();
            string result = caller.EntryPoint();

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        public void AssertCaller_GoodCallerGoodEntryPoint()
        {
            GoodCaller caller = new GoodCaller();
            string result = caller.GoodEntryPoint();

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertCaller_GoodCallerBadEntryPointThrowsAssertionException()
        {
            GoodCaller caller = new GoodCaller();
            string result = caller.BadEntryPoint();

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertCaller_BadCallerBadEntryPointThrowsAssertionException()
        {
            BadCaller caller = new BadCaller();
            string result = caller.BadEntryPoint();

            Assert.AreEqual( "ok", result );
        }
         
        [TestMethod]
        public void AssertNotCaller_NotBadCaller()
        {
            NotBadCaller caller = new NotBadCaller();
            string result = caller.EntryPoint();

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertNotCaller_NotGoodCallerThrowsAssertionException()
        {
            NotGoodCaller caller = new NotGoodCaller();
            string result = caller.EntryPoint();

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        public void AssertNotCaller_NotBadCallerNotBadEntryPoint()
        {
            NotBadCaller caller = new NotBadCaller();
            string result = caller.NotBadEntryPoint();

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertNotCaller_NotBadCallerNotGoodEntryPointThrowsAssertionException()
        {
            NotBadCaller caller = new NotBadCaller();
            string result = caller.NotGoodEntryPoint();

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        public void AssertCaller_Wildcard()
        {
            WildcardCaller caller = new WildcardCaller();
            string result = caller.GoodEntryPoint();

            Assert.AreEqual( "ok", result );
        }

        [TestMethod]
        [ExpectedException( typeof( AssertionException ) )]
        public void AssertNotCaller_Wildcard()
        {
            WildcardCaller caller = new WildcardCaller();
            string result = caller.BadEntryPoint();

            Assert.AreEqual( "ok", result );
        }

        public class WildcardCaller
        {
            public string GoodEntryPoint()
            {
                WildcardRestrictedTarget target = new WildcardRestrictedTarget();
                return target.TargetAllowedByWildcard();
            }

            public string BadEntryPoint()
            {
                WildcardRestrictedTarget target = new WildcardRestrictedTarget();
                return target.TargetNotAllowedByWildcard();
            }
        }

        public class WildcardRestrictedTarget
        {
            public string TargetAllowedByWildcard()
            {
                Condition.AssertCaller(
                    "SomeNamespace.SomeClass::SomeMethod",
                    "SomeNamespace.SomeOtherClass",
                    "_unittests.org.ncore.Common.ConditionTests+*" );
                return "ok";
            }

            public string TargetNotAllowedByWildcard()
            {
                Condition.AssertNotCaller(
                    "SomeNamespace.SomeClass::SomeMethod",
                    "SomeNamespace.SomeOtherClass",
                    "_unittests.org.ncore.Common.ConditionTests+*" );
                return "ok";
            }
        }

        public class GoodCaller
        {
            public string EntryPoint()
            {
                RestrictedTarget target = new RestrictedTarget();
                return target.TargetMethodByTypeName();
            }

            public string GoodEntryPoint()
            {
                RestrictedTarget target = new RestrictedTarget();
                return target.TargetMethodByMethodName();
            }

            public string BadEntryPoint()
            {
                RestrictedTarget target = new RestrictedTarget();
                return target.TargetMethodByMethodName();
            }
        }

        public class BadCaller
        {
            public string EntryPoint()
            {
                RestrictedTarget target = new RestrictedTarget();
                return target.TargetMethodByTypeName();
            }

            public string BadEntryPoint()
            {
                RestrictedTarget target = new RestrictedTarget();
                return target.TargetMethodByMethodName();
            }
        }

        public class NotGoodCaller
        {
            public string EntryPoint()
            {
                RestrictedTarget target = new RestrictedTarget();
                return target.TargetMethodByNotTypeName();
            }

            public string NotGoodEntryPoint()
            {
                RestrictedTarget target = new RestrictedTarget();
                return target.TargetMethodByNotMethodName();
            }
        }

        public class NotBadCaller
        {
            public string EntryPoint()
            {
                RestrictedTarget target = new RestrictedTarget();
                return target.TargetMethodByNotTypeName();
            }

            public string NotBadEntryPoint()
            {
                RestrictedTarget target = new RestrictedTarget();
                return target.TargetMethodByNotMethodName();
            }

            public string NotGoodEntryPoint()
            {
                RestrictedTarget target = new RestrictedTarget();
                return target.TargetMethodByNotMethodName();
            }
        }

        public class RestrictedTarget
        {
            public string TargetMethodByMethodName()
            {
                Condition.AssertCaller(
                    "SomeNamespace.SomeClass::SomeMethod",
                    "SomeNamespace.SomeOtherClass",
                    "_unittests.org.ncore.Common.ConditionTests+GoodCaller::GoodEntryPoint" );
                return "ok";
            }

            public string TargetMethodByTypeName()
            {
                Condition.AssertCaller(
                    "SomeNamespace.SomeClass::SomeMethod",
                    "SomeNamespace.SomeOtherClass",
                    "_unittests.org.ncore.Common.ConditionTests+GoodCaller" );
                return "ok";
            }

            public string TargetMethodByNotMethodName()
            {
                Condition.AssertNotCaller(
                    "SomeNamespace.SomeClass::SomeMethod",
                    "SomeNamespace.SomeOtherClass",
                    "_unittests.org.ncore.Common.ConditionTests+NotBadCaller::NotGoodEntryPoint" );
                return "ok";
            }

            public string TargetMethodByNotTypeName()
            {
                Condition.AssertNotCaller(
                    "SomeNamespace.SomeClass::SomeMethod",
                    "SomeNamespace.SomeOtherClass",
                    "_unittests.org.ncore.Common.ConditionTests+NotGoodCaller" );
                return "ok";
            }
        }
        #endregion
    }
}
