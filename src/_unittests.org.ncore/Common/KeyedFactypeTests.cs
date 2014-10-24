using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Common;

namespace _unittests.org.ncore.Common
{
    /// <summary>
    /// Summary description for KeyedFactypeTests
    /// </summary>
    [TestClass]
    public class KeyedFactypeTests
    {
        public KeyedFactypeTests()
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
        // NOTE: Expository, not a test.  Demonstrates basic use of KeyedFactype.  JF
        public void Demo_SimpleStatusKeyedFactype()
        {
            // NOTE: Directly assignable like a regular Enum.
            SimpleStatusKeyedFactype closed = SimpleStatusKeyedFactype.Closed;
            Assert.AreEqual( SimpleStatusKeyedFactype.Closed, closed );

            // NOTE: Implicitly casts to string (something normal Enums don't do). Same as: string moniker = SimpleStatusKeyedFactype.Closed.Moniker;
            string moniker = SimpleStatusKeyedFactype.Closed;
            Assert.AreEqual( "Closed", moniker );

            // NOTE: Implicitly casts to int (something normal Enums don't do). Same as: int key = SimpleStatusKeyedFactype.Closed.Key;
            int key = SimpleStatusKeyedFactype.Closed;
            Assert.AreEqual( 3, key );

            // NOTE: Not instantiable (except privately) so whole thing is effectively static at runtime (like an enum)
            //SimpleStatusKeyedFactype d = new SimpleStatusKeyedFactype(); // This won't compile

            // NOTE: Parses strings and ints
            SimpleStatusKeyedFactype open = KeyedFactype<Int32>.Parse<SimpleStatusKeyedFactype>( 2 );
            SimpleStatusKeyedFactype cancelled = KeyedFactype<Int32>.Parse<SimpleStatusKeyedFactype>( "Cancelled" );

            // NOTE: Parses strings and ints using convenience methods.
            open = SimpleStatusKeyedFactype.Parse( 2 );
            cancelled = SimpleStatusKeyedFactype.Parse( "Cancelled" );

            // NOTE: Implicitly cast from an int or a string using convenience methods.
            open = (SimpleStatusKeyedFactype)2;
            cancelled = (SimpleStatusKeyedFactype)"Cancelled";

            // NOTE: ToString() works like an enum, returning the expression (Moniker) value.
            string returned = SimpleStatusKeyedFactype.Returned.ToString();
        }

        [TestMethod]
        // NOTE: Expository, not a test.  Demonstrates some advanced uses of KeyedFactype.  JF
        public void Demo_ComplexStatusKeyedFactype()
        {
            // NOTE: We can build custom lists.
            List<ComplexStatusKeyedFactype> activeStatuses = ComplexStatusKeyedFactype.GetActive();

            // NOTE: We can even sort using custom comparers.
            List<ComplexStatusKeyedFactype> allStatuses = ComplexStatusKeyedFactype.GetAll();
            allStatuses.Sort( ComplexStatusKeyedFactype.CompareByDisplayText );
        }


        [TestMethod]
        public void CanParse_Base()
        {
            bool canParse1 = KeyedFactype<Int32>.CanParse<SimpleStatusKeyedFactype>( 1 );
            bool canParse57 = KeyedFactype<Int32>.CanParse<SimpleStatusKeyedFactype>( 57 );

            Assert.IsTrue( canParse1 );
            Assert.IsFalse( canParse57 );
        }

        [TestMethod]
        public void CanParse_Derived()
        {
            bool canParse1 = SimpleStatusKeyedFactype.CanParse( 1 );
            bool canParse57 = SimpleStatusKeyedFactype.CanParse( 57 );

            Assert.IsTrue( canParse1 );
            Assert.IsFalse( canParse57 );
        }

        [TestMethod]
        public void AssignFactypeToString()
        {
            // NOTE: Convert from our factype to a string (i.e. the moniker)
            string open = SimpleStatusKeyedFactype.Open;
            Assert.AreEqual( "Open", open );
        }

        [TestMethod]
        public void AssignFactypeToKeyType()
        {
            // NOTE: Convert from our factype to an int (i.e. the key)
            Int32 open = SimpleStatusKeyedFactype.Open;
            Assert.AreEqual( 2, open );
        }

        [TestMethod]
        public void RetrieveFactypeMoniker()
        {
            // NOTE: Factype .Moniker is the same as the static instance name
            string open = SimpleStatusKeyedFactype.Open.Moniker;
            Assert.AreEqual( "Open", open );
        }

        [TestMethod]
        public void RetrieveFactypeKey()
        {
            // NOTE: Factype .Key is the unique key of the value (similar to the numeric value in an enumeration).  JF
            Int32 open = SimpleStatusKeyedFactype.Open.Key;
            Assert.AreEqual( 2, open );
        }

        [TestMethod]
        public void CastStringToFactype()
        {
            // NOTE: Implicit cast from a string (our moniker) to our factype
            SimpleStatusKeyedFactype factype = "Open";
            Assert.AreEqual( SimpleStatusKeyedFactype.Open, factype );
        }

        [TestMethod]
        public void CastIntToFactype()
        {
            // NOTE: Implicit cast from an int (our key) to our factype
            SimpleStatusKeyedFactype factype = 2;
            Assert.AreEqual( SimpleStatusKeyedFactype.Open, factype );
        }


        [TestMethod]
        [ExpectedException( typeof( System.ArgumentException ), "Value open is not a member of _unittests.org.ncore.Common.SimpleStatusKeyedFactype." )]
        public void CastStringThrowsOnMonikerCaseSensitivity()
        {
            // NOTE: Wrong.  Should be "Open", not "open".  JF
            SimpleStatusKeyedFactype factype = "open";
        }

        [TestMethod]
        [ExpectedException( typeof( System.ArgumentException ), "Value open is not a member of _unittests.org.ncore.Common.SimpleStatusKeyedFactype." )]
        public void CastIntThrowsOnInvalidValue()
        {
            // NOTE: Wrong.  57 is not a valid Key value.  JF
            SimpleStatusKeyedFactype factype = 57;
        }

        [TestMethod]
        public void GetAll_BaseAndDerived()
        {
            // NOTE: Get the list of SimpleStatusFactype items.
            List<SimpleStatusKeyedFactype> factypesBase = Factype.GetAll<SimpleStatusKeyedFactype>();

            // NOTE: or you can wrap the above line of code in a GetAll() method 
            //  in your Factype class and make life a little easier for your caller.
            List<SimpleStatusKeyedFactype> factypesDerived = SimpleStatusKeyedFactype.GetAll();

            Assert.IsTrue( factypesBase.SequenceEqual( factypesDerived ) );
        }

        [TestMethod]
        public void GetMonikers_BaseAndDerived()
        {

            // NOTE: Get the monikers using the base.  JF
            string[] monikersBase = Factype.GetMonikers<SimpleStatusKeyedFactype>();
            //  OR the specific type.  JF
            string[] monikersDerived = SimpleStatusKeyedFactype.GetMonikers();

            Assert.IsTrue( monikersBase.SequenceEqual( monikersDerived ) );
        }

        [TestMethod]
        public void GetKeys_BaseAndDerived()
        {

            // NOTE: Get the keys using the base.  JF
            Int32[] keysBase = KeyedFactype<Int32>.GetKeys<SimpleStatusKeyedFactype>();
            //  OR the specific type.  JF
            Int32[] keysDerived = SimpleStatusKeyedFactype.GetKeys();

            Assert.IsTrue( keysBase.SequenceEqual( keysDerived ) );
        }

        [TestMethod]
        [ExpectedException( typeof( System.TypeInitializationException ) )]
        public void ViolateKeyUniqueness()
        {
            // NOTE: This next line initializes the static factype members and causes a dupe key exception.  JF
            int dupeKey = DupeKeysFactype.KeyFourDupe;
        }

        [TestMethod]
        // NOTE: Expository, not a test.  Demonstrates using KeyedFactype as a "flag" type enumeration.  JF
        public void Demo_ToppingsFlagtype()
        {
            // NOTE: Fully supports bitwise logic used in flag-type enums.  Very cool.  Of course this does
            //  require that the key values are bitwise exclusive (for lack of a better term) - i.e. 1,2,4,8,16,etc. JF
            int toppingFlags = ToppingsFlagtype.Anchovies | ToppingsFlagtype.CanadianBacon | ToppingsFlagtype.Pepperoni;

            if( !ToppingsFlagtype.CanParse( toppingFlags ) )
            {
                List<ToppingsFlagtype> toppings = ToppingsFlagtype.ParseBitwise( toppingFlags );
            }
        }
    }

    public class SimpleStatusKeyedFactype : KeyedFactype<Int32>
    {
        public static readonly SimpleStatusKeyedFactype New = new SimpleStatusKeyedFactype( 1 );
        public static readonly SimpleStatusKeyedFactype Open = new SimpleStatusKeyedFactype( 2 );
        public static readonly SimpleStatusKeyedFactype Closed = new SimpleStatusKeyedFactype( 3 );
        public static readonly SimpleStatusKeyedFactype Cancelled = new SimpleStatusKeyedFactype( 4 );
        public static readonly SimpleStatusKeyedFactype Returned = new SimpleStatusKeyedFactype( 5 );
        public static readonly SimpleStatusKeyedFactype Forwarded = new SimpleStatusKeyedFactype( 6 );

        private SimpleStatusKeyedFactype( Int32 key )
            : base( key )
        {
        }

        // NOTE: Optional additions to the class to add functionality.  These are all just wrapper methods.  JF
        public static implicit operator SimpleStatusKeyedFactype( string moniker )
        {
            return SimpleStatusKeyedFactype.Parse<SimpleStatusKeyedFactype>( moniker );
        }

        public static implicit operator SimpleStatusKeyedFactype( Int32 key )
        {
            return SimpleStatusKeyedFactype.Parse<SimpleStatusKeyedFactype>( key );
        }

        public static List<SimpleStatusKeyedFactype> GetAll()
        {
            return SimpleStatusKeyedFactype.GetAll<SimpleStatusKeyedFactype>();
        }

        public static string[] GetMonikers()
        {
            return Factype.GetMonikers<SimpleStatusKeyedFactype>();
        }

        public static Int32[] GetKeys()
        {
            return KeyedFactype<Int32>.GetKeys<SimpleStatusKeyedFactype>();
        }

        public static bool CanParse( int key )
        {
            return KeyedFactype<Int32>.CanParse<SimpleStatusKeyedFactype>( key );
        }

        public static SimpleStatusKeyedFactype Parse( int key )
        {
            return KeyedFactype<Int32>.Parse<SimpleStatusKeyedFactype>( key );
        }

        public static SimpleStatusKeyedFactype Parse( string moniker )
        {
            return KeyedFactype<Int32>.Parse<SimpleStatusKeyedFactype>( moniker );
        }
    }

    public class ComplexStatusKeyedFactype : KeyedFactype<Int32>
    {
        public static readonly ComplexStatusKeyedFactype New = new ComplexStatusKeyedFactype( 1, "New", true );
        public static readonly ComplexStatusKeyedFactype Open = new ComplexStatusKeyedFactype( 2, "Open", true );
        public static readonly ComplexStatusKeyedFactype Closed = new ComplexStatusKeyedFactype( 3, "Closed", false );
        public static readonly ComplexStatusKeyedFactype Cancelled = new ComplexStatusKeyedFactype( 4, "Manually Cancelled", false );
        public static readonly ComplexStatusKeyedFactype Returned = new ComplexStatusKeyedFactype( 5, "Returned to Sender", false );
        public static readonly ComplexStatusKeyedFactype Forwarded = new ComplexStatusKeyedFactype( 6, "Forwarded to New Receipient", true );

        private ComplexStatusKeyedFactype( Int32 key, string displayText, bool isActive )
            : base( key )
        {
            this.DisplayText = displayText;
            this.IsActive = isActive;
        }

        // NOTE: Optional additions to the class to add functionality.  These are all just wrapper methods.  JF
        public static implicit operator ComplexStatusKeyedFactype( string moniker )
        {
            return ComplexStatusKeyedFactype.Parse<ComplexStatusKeyedFactype>( moniker );
        }

        public static implicit operator ComplexStatusKeyedFactype( Int32 key )
        {
            return ComplexStatusKeyedFactype.Parse<ComplexStatusKeyedFactype>( key );
        }

        public static List<ComplexStatusKeyedFactype> GetAll()
        {
            return Factype.GetAll<ComplexStatusKeyedFactype>();
        }

        public static string[] GetMonikers()
        {
            return Factype.GetMonikers<ComplexStatusKeyedFactype>();
        }

        public static Int32[] GetKeys()
        {
            return KeyedFactype<Int32>.GetKeys<ComplexStatusKeyedFactype>();
        }

        public static bool CanParse( int key )
        {
            return KeyedFactype<Int32>.CanParse<ComplexStatusKeyedFactype>( key );
        }

        public static ComplexStatusKeyedFactype Parse( int key )
        {
            return KeyedFactype<Int32>.Parse<ComplexStatusKeyedFactype>( key );
        }

        public static ComplexStatusKeyedFactype Parse( string moniker )
        {
            return KeyedFactype<Int32>.Parse<ComplexStatusKeyedFactype>( moniker );
        }

        public static List<ComplexStatusKeyedFactype> GetActive()
        {
            // NOTE: This could be cached in a private static readonly list since it won't change.
            FactypeFilterCriteria<ComplexStatusKeyedFactype> criteria = new FactypeFilterCriteria<ComplexStatusKeyedFactype>( _isActive );
            return Factype.GetFiltered<ComplexStatusKeyedFactype>( criteria );
        }

        public static Int32 CompareByDisplayText( ComplexStatusKeyedFactype itemA, ComplexStatusKeyedFactype itemB )
        {
            return ( itemA.DisplayText.CompareTo( itemB.DisplayText ) );
        }

        private static bool _isActive( ComplexStatusKeyedFactype complexStatusFactype32 )
        {
            return complexStatusFactype32.IsActive;
        }

        // NOTE: Additional members of this specific Factype
        public string DisplayText { get; protected set; }
        public bool IsActive { get; protected set; }
    }

    // TODO: Would be cool to generalize this into some kind of "BitwiseFactype" or
    //  "Flagtype" base type.  But how?  The generics approach doesn't seem to work 
    //  because it's not possible to constrain generics to only integral types so the
    //  bitwise stuff on a generic won't compile (even with a "where T : struct" it
    //  could still be a non-integral type that doesn't support bitwise operations.)
    //  Unfortunately this is just the way .NET works (though there are a lot of
    //  discussions out there about this limitation and the challenges of fixing
    //  it.)  Anyway, for now if you want a flag-like Factype you can just derive
    //  from KeyedFactype and implement ParseBitwise() as shown below in your 
    //  derived class.  JF
    // TODO: Experiment with KeyedFactype<MyFlagEnum> where MyFlagEnum is an enum
    //  decorated with [FlagsAttribute].  At the very least this will ensure that
    //  the Key values are all legit bit flags.  JF
    public class ToppingsFlagtype : KeyedFactype<Int32>
    {
        public static ToppingsFlagtype Cheese = new ToppingsFlagtype( 1 );
        public static ToppingsFlagtype Pepperoni = new ToppingsFlagtype( 2 );
        public static ToppingsFlagtype Mushrooms = new ToppingsFlagtype( 4 );
        public static ToppingsFlagtype Pineapple = new ToppingsFlagtype( 8 );
        public static ToppingsFlagtype CanadianBacon = new ToppingsFlagtype( 16 );
        public static ToppingsFlagtype BlackOlives = new ToppingsFlagtype( 32 );
        public static ToppingsFlagtype Anchovies = new ToppingsFlagtype( 64 );

        private ToppingsFlagtype( Int32 key )
            : base( key )
        {
        }

        // NOTE: Optional additions to the class to add functionality.  These are all just wrapper methods.  JF
        public static ToppingsFlagtype Parse( Int32 key )
        {
            return KeyedFactype<Int32>.Parse<ToppingsFlagtype>( key );
        }

        public static bool CanParse( Int32 key )
        {
            return KeyedFactype<Int32>.CanParse<ToppingsFlagtype>( key );
        }

        // NOTE: Custom bitwise parsing of a set of ORed keys.  This is pretty cool.  JF
        public static List<ToppingsFlagtype> ParseBitwise( Int32 keys )
        {
            List<ToppingsFlagtype> all = GetAll<ToppingsFlagtype>();
            List<ToppingsFlagtype> bitsOn = new List<ToppingsFlagtype>();
            foreach( ToppingsFlagtype item in all )
            {
                int bit = item.Key & keys;
                if( bit != 0 )
                {
                    bitsOn.Add( item );
                }
            }
            return bitsOn;
        }
    }

    public class DupeKeysFactype : KeyedFactype<Int32>
    {
        public static DupeKeysFactype KeyOne = new DupeKeysFactype( 1 );
        public static DupeKeysFactype KeyTwo = new DupeKeysFactype( 2 );
        public static DupeKeysFactype KeyThree = new DupeKeysFactype( 3 );
        public static DupeKeysFactype KeyFour = new DupeKeysFactype( 4 );
        public static DupeKeysFactype KeyFourDupe = new DupeKeysFactype( 4 );

        private DupeKeysFactype( Int32 key )
            : base( key )
        {
        }
    }
}
