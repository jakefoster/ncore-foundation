using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Common;
using org.ncore.Extensions;

namespace _unittests.org.ncore.Common
{
    [TestClass]
    public class FactypeTests
    {
        public FactypeTests()
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


        // TODO: Implement some "DEMO" type expository tests that concisely summarize the basic
        //  uses of Factypes.  See the KeyedFactypeTests for examples.  JF

        [TestMethod]
        public void CanParse()
        {
            // NOTE: Verify that Moniker is valid using CanParse().  JF
            bool canParseOpen1 = Factype.CanParse<SimpleStatusFactype>( "Open" );
            // NOTE: Monikers are case-sensitive so this returns false.  JF
            bool canParseOpen2 = Factype.CanParse<SimpleStatusFactype>( "open" );
            // NOTE: "Invalid" isn't a valid moniker here so this returns false.  Clever huh?  JF
            bool canParseInvalid = Factype.CanParse<SimpleStatusFactype>( "Invalid" );

            Assert.AreEqual( true, canParseOpen1 );
            Assert.AreEqual( false, canParseOpen2 );
            Assert.AreEqual( false, canParseInvalid );
        }

        [TestMethod]
        public void AssignFactypeToString()
        {
            // NOTE: Convert from our factype to a string (i.e. the moniker)
            string open = SimpleStatusFactype.Open;
            Assert.AreEqual( "Open", open );
        }

        [TestMethod]
        public void RetrieveFactypeMoniker()
        {
            // NOTE: Factype .Moniker is the same as the static instance name
            string open = SimpleStatusFactype.Open.Moniker;
            Assert.AreEqual( "Open", open );
        }

        [TestMethod]
        public void CastStringToFactype()
        {
            // NOTE: Cast from a string (our moniker) to our factype
            SimpleStatusFactype factype = "Open";
            Assert.AreEqual( SimpleStatusFactype.Open, factype );
        }

        [TestMethod]
        [ExpectedException( typeof( System.ArgumentException ), "Value open is not a member of _unittests.org.ncore.Common.SimpleStatusFactype." )]
        public void CastStringThrowsOnMonikerCaseSensitivity()
        {
            // NOTE: Wrong.  Should be "Open", not "open".  JF
            SimpleStatusFactype factype = "open";
        }

        [TestMethod]
        public void GetAll_BaseAndDerived()
        {
            // NOTE: Get the list of SimpleStatusFactype items.
            List<SimpleStatusFactype> factypesBase = Factype.GetAll<SimpleStatusFactype>();

            // NOTE: or you can wrap the above line of code in a GetAll() method 
            //  in your Factype class and make life a little easier for your caller.
            List<SimpleStatusFactype> factypesDerived = SimpleStatusFactype.GetAll();

            Assert.IsTrue( factypesBase.SequenceEqual( factypesDerived ) );
        }

        [TestMethod]
        public void GetMonikers_BaseAndDerived()
        {

            // NOTE: Get the monikers using the base.  JF
            string[] monikersBase = Factype.GetMonikers<SimpleStatusFactype>();
            //  OR the specific type.  JF
            string[] monikersDerived = SimpleStatusFactype.GetMonikers();

            Assert.IsTrue( monikersBase.SequenceEqual( monikersDerived ) );
        }

        [TestMethod]
        public void GetDescriptors()
        {
            string[] descriptors = ComplexStatusFactype.GetDescriptors();
            descriptors = descriptors.OrderBy( d => d ).ToArray();
            Assert.AreEqual( 4, descriptors.Length );
            Assert.AreEqual( "A completed and manually closed instance.", descriptors[ 0 ] );
            Assert.AreEqual( "A new, unopened instance.", descriptors[ 1 ] );
            Assert.AreEqual( "An instance that has been manually cancelled.", descriptors[ 2 ] );
            Assert.AreEqual( "An instance that has been opened at least once.", descriptors[ 3 ] );
        }
    }

    public class SimpleStatusFactype : Factype
    {
        // NOTE: "Initializers" - Create out immutable, static instances.  These are the functional 
        //  equivalent of an enumeration.  E.g. SimpleStatusFactype.Closed.  Cool!  JF
        public static readonly SimpleStatusFactype New = new SimpleStatusFactype();
        public static readonly SimpleStatusFactype Open = new SimpleStatusFactype();
        public static readonly SimpleStatusFactype Closed = new SimpleStatusFactype();
        public static readonly SimpleStatusFactype Cancelled = new SimpleStatusFactype();

        private SimpleStatusFactype() : base()
        {
        }

        // NOTE: Optional additions to the class to add functionality.  These are all just wrapper methods.  JF
        public static implicit operator SimpleStatusFactype( string moniker )
        {
            return SimpleStatusFactype.Parse<SimpleStatusFactype>( moniker );
        }

        public static List<SimpleStatusFactype> GetAll()
        {
            return SimpleStatusFactype.GetAll<SimpleStatusFactype>();
        }

        public static string[] GetMonikers()
        {
            return SimpleStatusFactype.GetMonikers<SimpleStatusFactype>();
        }
    }

    public class ComplexStatusFactype : Factype
    {
        public static readonly ComplexStatusFactype New = new ComplexStatusFactype( "A new, unopened instance." );
        public static readonly ComplexStatusFactype Open = new ComplexStatusFactype( "An instance that has been opened at least once." );
        public static readonly ComplexStatusFactype Closed = new ComplexStatusFactype( "A completed and manually closed instance." );
        public static readonly ComplexStatusFactype Cancelled = new ComplexStatusFactype( "An instance that has been manually cancelled." );

        private ComplexStatusFactype( string descriptor )
            : base()
        {
            this.Descriptor = descriptor;
        }

        // NOTE: Optional additions to the class to add functionality.  These are all just wrapper methods.  JF
        public static implicit operator ComplexStatusFactype( string moniker )
        {
            return ComplexStatusFactype.Parse<ComplexStatusFactype>( moniker );
        }

        public static List<ComplexStatusFactype> GetAll()
        {
            return ComplexStatusFactype.GetAll<ComplexStatusFactype>();
        }

        public static string[] GetMonikers()
        {
            return ComplexStatusFactype.GetMonikers<ComplexStatusFactype>();
        }

        public static string[] GetDescriptors()
        {
            List<ComplexStatusFactype> list = ComplexStatusFactype.GetAll();
            string[] descriptors = new string[ list.Count ];
            for( int i = 0; i < descriptors.Length; ++i )
            {
                descriptors[ i ] = list[ i ].Descriptor;
            }
            return descriptors;
        }

        public string Descriptor{ get; protected set; }
    }
}
