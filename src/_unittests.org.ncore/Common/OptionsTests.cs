using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Common;

namespace _unittests.org.ncore.Common
{
    /// <summary>
    /// Summary description for OptionsTests
    /// </summary>
    [TestClass]
    public class OptionsTests
    {
        public OptionsTests()
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
        public void Construct_WithArgs()
        {
            string[] myArgs = new string[] { "/Mode:Debug", "/Path:c:\\myapp\\" };
            Options options = new Options( myArgs );

            Assert.AreEqual( 2, options.Count );
            Assert.IsTrue( options.Keys.Contains( "Mode" ) );
            Assert.AreEqual( "Debug", options[ "Mode" ] );
            Assert.IsTrue( options.Keys.Contains( "Path" ) );
            Assert.AreEqual( "c:\\myapp\\", options[ "Path" ] );
        }

        [TestMethod]
        public void ConstructGeneric_WithArgs()
        {
            string[] myArgs = new string[] { "/Mode:Debug", "/Path:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs );

            Assert.AreEqual( 2, options.Count );
            Assert.IsTrue( options.Keys.Contains( "Mode" ) );
            Assert.AreEqual( "Debug", options[ "Mode" ] );
            Assert.IsTrue( options.Keys.Contains( "Path" ) );
            Assert.AreEqual( "c:\\myapp\\", options[ "Path" ] );
        }

        [TestMethod]
        public void GetConfiguration_MySimpleConfig_ExactMatch()
        {
            string[] myArgs = new string[] { "/Mode:Debug", "/Path:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs );
            MySimpleConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( "Debug", config.Mode );
            Assert.AreEqual( "c:\\myapp\\", config.Path );
        }

        [TestMethod]
        public void GetConfiguration_MySimpleConfig_StartsWithMatch()
        {
            string[] myArgs = new string[] { "/m:Debug", "/pat:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs );
            MySimpleConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( "Debug", config.Mode );
            Assert.AreEqual( "c:\\myapp\\", config.Path );
        }

        [TestMethod]
        public void GetConfiguration_MySimpleConfig_AttributeMatch()
        {
            string[] myArgs = new string[] { "/X:Debug", "/Path:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs );
            MySimpleConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( "Debug", config.Mode );
            Assert.AreEqual( "c:\\myapp\\", config.Path );
        }

        [TestMethod]
        public void GetConfiguration_MySimpleConfig_WithDashes()
        {
            string[] myArgs = new string[] { "-Mode:Debug", "-Path:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs );
            MySimpleConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( "Debug", config.Mode );
            Assert.AreEqual( "c:\\myapp\\", config.Path );
        }

        [TestMethod]
        public void GetConfiguration_MySimpleConfig_WithoutSwitchSpecifier()
        {
            string[] myArgs = new string[] { "Mode:Debug", "Path:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs );
            MySimpleConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( "Debug", config.Mode );
            Assert.AreEqual( "c:\\myapp\\", config.Path );
        }

        [TestMethod]
        public void GetConfiguration_MySimpleConfig_WithEquals()
        {
            string[] myArgs = new string[] { "/Mode=Debug", "/Path=c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs );
            MySimpleConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( "Debug", config.Mode );
            Assert.AreEqual( "c:\\myapp\\", config.Path );
        }

        [TestMethod]
        public void GetConfiguration_MySimpleConfig_MatchTypeExact()
        {
            string[] myArgs = new string[] { "/Mode:Debug", "/Path:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs )
                                                  {
                                                      MatchType = OptionMatchType.Exact
                                                  };
            MySimpleConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( "Debug", config.Mode );
            Assert.AreEqual( "c:\\myapp\\", config.Path );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ), "The specified argument is not valid." )]
        public void GetConfiguration_MySimpleConfig_MatchTypeExact_Throws()
        {
            string[] myArgs = new string[] { "/mode:Debug", "/P:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs )
                                                  {
                                                      MatchType = OptionMatchType.Exact
                                                  };
            MySimpleConfig config = options.GetConfiguration();
        }

        [TestMethod]
        public void GetConfiguration_MySimpleConfig_MatchTypeStartsWith()
        {
            string[] myArgs = new string[] { "/m:Debug", "/pa:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs )
                                                  {
                                                      MatchType = OptionMatchType.StartsWith
                                                  };
            MySimpleConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( "Debug", config.Mode );
            Assert.AreEqual( "c:\\myapp\\", config.Path );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ), "The specified argument is not valid." )]
        public void GetConfiguration_MySimpleConfig_MatchTypeStartsWith_Throws()
        {
            string[] myArgs = new string[] { "/X:Debug", "/Y:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs )
                                                  {
                                                      MatchType = OptionMatchType.StartsWith
                                                  };
            MySimpleConfig config = options.GetConfiguration();
        }

        [TestMethod]
        public void GetConfiguration_MySimpleConfig_MatchTypeAttribute()
        {
            string[] myArgs = new string[] { "/X:Debug", "/Y:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs )
                                                  {
                                                      MatchType = OptionMatchType.Attribute
                                                  };
            MySimpleConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( "Debug", config.Mode );
            Assert.AreEqual( "c:\\myapp\\", config.Path );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ), "The specified argument is not valid." )]
        public void GetConfiguration_MySimpleConfig_MatchTypeAttribute_Throws()
        {
            string[] myArgs = new string[] { "/Mode:Debug", "/p:c:\\myapp\\" };
            Options<MySimpleConfig> options = new Options<MySimpleConfig>( myArgs )
                                                  {
                                                      MatchType = OptionMatchType.Attribute
                                                  };
            MySimpleConfig config = options.GetConfiguration();
        }

        [TestMethod]
        public void GetConfiguration_MyEnumConfig()
        {
            string[] myArgs = new string[] { "/Enum:One" };
            Options<MyEnumConfig> options = new Options<MyEnumConfig>( myArgs );
            MyEnumConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( AnEnum.One, config.Enum );
        }

        [TestMethod]
        public void GetConfiguration_MyBooleanConfig_EmptySwitch()
        {
            // NOTE: Special case convenience scenario: where the type is a bool and no value is specified, the
            //  mere presence of the switch means "true".  JF
            string[] myArgs = new string[] { "/Active:" };
            Options<MyBooleanConfig> options = new Options<MyBooleanConfig>( myArgs );
            MyBooleanConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( true, config.Active );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ), "Requested value 'NonEnumValue' was not found." )]
        public void GetConfiguration_MyEnumConfig_InvalidValue()
        {
            string[] myArgs = new string[] { "/Enum:NonEnumValue" };
            Options<MyEnumConfig> options = new Options<MyEnumConfig>( myArgs );
            MyEnumConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual( AnEnum.One, config.Enum );
        }

        [TestMethod]
        public void GetConfiguration_MyComplexConfig()
        {
            string[] myArgs = new string[]
                                  {
                                      "/mby:255",
                                      "/mbyn:255",
                                      "/msby:-128",
                                      "/msbyn:-128",
                                      "/mi16:-32768",
                                      "/mi16n:-32768",
                                      "/mui16:65535",
                                      "/mui16n:65535",
                                      "/mi32:-2147483648",
                                      "/mi32n:-2147483648",
                                      "/mui32:4294967295",
                                      "/mui32n:4294967295",
                                      "/mi64:-922337203685477508",
                                      "/mi64n:-922337203685477508",
                                      "/mui64:18446744073709551615",
                                      "/mui64n:18446744073709551615",
                                      "/ms:3.402823e38",
                                      "/msn:3.402823e38",
                                      "/md:1.79769313486231570E+308",
                                      "/mdn:1.79769313486231570E+308",
                                      "/mc:\u0001",
                                      "/mcn:\u0001",
                                      "/mb:true",
                                      "/mbn:true",
                                      "/mde:.0123456789",
                                      "/mden:.0123456789",
                                      "/mdt:2011-05-16 9:42 PM",
                                      "/mdtn:2011-05-16 9:42 PM",
                                      "/me:One",
                                      "/men:One",
                                      "/mst:I am a string"
                                  };
            Options<MyComplexConfig> options = new Options<MyComplexConfig>( myArgs );
            MyComplexConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );
            Assert.AreEqual<Byte>( 255, config.MyByte );
            Assert.AreEqual<Nullable<Byte>>( 255, config.MyByteNullable );
            Assert.AreEqual<SByte>( -128, config.MySByte );
            Assert.AreEqual<Nullable<SByte>>( -128, config.MySByteNullable );

            Assert.AreEqual<Int16>( -32768, config.MyInt16 );
            Assert.AreEqual<Nullable<Int16>>( -32768, config.MyInt16Nullable );
            Assert.AreEqual<UInt16>( 65535, config.MyUInt16 );
            Assert.AreEqual<Nullable<UInt16>>( 65535, config.MyUInt16Nullable );

            Assert.AreEqual<Int32>( -2147483648, config.MyInt32 );
            Assert.AreEqual<Nullable<Int32>>( -2147483648, config.MyInt32Nullable );
            Assert.AreEqual<UInt32>( 4294967295, config.MyUInt32 );
            Assert.AreEqual<Nullable<UInt32>>( 4294967295, config.MyUInt32Nullable );

            Assert.AreEqual<Int64>( -922337203685477508, config.MyInt64 );
            Assert.AreEqual<Nullable<Int64>>( -922337203685477508, config.MyInt64Nullable );
            Assert.AreEqual<UInt64>( 18446744073709551615, config.MyUInt64 );
            Assert.AreEqual<Nullable<UInt64>>( 18446744073709551615, config.MyUInt64Nullable );

            Assert.AreEqual<Single>( (Single)3.402823e38, config.MySingle );
            Assert.AreEqual<Nullable<Single>>( (Single)3.402823e38, config.MySingleNullable );

            Assert.AreEqual<Double>( 1.79769313486231570E+308, config.MyDouble );
            Assert.AreEqual<Nullable<Double>>( 1.79769313486231570E+308, config.MyDoubleNullable );

            Assert.AreEqual<Char>( (Char)1, config.MyChar );
            Assert.AreEqual<Nullable<Char>>( (Char)1, config.MyCharNullable );

            Assert.AreEqual( true, config.MyBoolean );
            Assert.AreEqual( true, config.MyBooleanNullable );

            Assert.AreEqual<Decimal>( (Decimal).0123456789, config.MyDecimal );
            Assert.AreEqual <Nullable<Decimal>>( (Decimal).0123456789, config.MyDecimalNullable );

            Assert.AreEqual( DateTime.Parse( "2011-05-16 9:42 PM" ), config.MyDateTime );
            Assert.AreEqual( DateTime.Parse( "2011-05-16 9:42 PM" ), config.MyDateTimeNullable );

            Assert.AreEqual( AnEnum.One, config.MyEnum );
            Assert.AreEqual( AnEnum.One, config.MyEnumNullable );

            Assert.AreEqual( "I am a string", config.MyString );
        }

        [TestMethod]
        public void GetConfiguration_MyComplexConfig_NullNullables()
        {
            string[] myArgs = new string[]
                                  {
                                      "/mby:255",
                                      "/mbyn:",
                                      "/msby:-128",
                                      "/msbyn:",
                                      "/mi16:-32768",
                                      "/mi16n:",
                                      "/mui16:65535",
                                      "/mui16n:",
                                      "/mi32:-2147483648",
                                      "/mi32n:",
                                      "/mui32:4294967295",
                                      "/mui32n:",
                                      "/mi64:-922337203685477508",
                                      "/mi64n:",
                                      "/mui64:18446744073709551615",
                                      "/mui64n:",
                                      "/ms:3.402823e38",
                                      "/msn:",
                                      "/md:1.79769313486231570E+308",
                                      "/mdn:",
                                      "/mc:\u0001",
                                      "/mcn:",
                                      "/mb:true",
                                      "/mbn:",
                                      "/mde:.0123456789",
                                      "/mden:",
                                      "/mdt:2011-05-16 9:42 PM",
                                      "/mdtn:",
                                      "/me:One",
                                      "/men:"
                                  };
            Options<MyComplexConfig> options = new Options<MyComplexConfig>( myArgs );
            MyComplexConfig config = options.GetConfiguration();

            Assert.IsNotNull( config );

            Assert.IsNotNull( config );
            Assert.AreEqual<Byte>( 255, config.MyByte );
            Assert.AreEqual<Nullable<Byte>>( null, config.MyByteNullable );
            Assert.AreEqual<SByte>( -128, config.MySByte );
            Assert.AreEqual<Nullable<SByte>>( null, config.MySByteNullable );

            Assert.AreEqual<Int16>( -32768, config.MyInt16 );
            Assert.AreEqual<Nullable<Int16>>( null, config.MyInt16Nullable );
            Assert.AreEqual<UInt16>( 65535, config.MyUInt16 );
            Assert.AreEqual<Nullable<UInt16>>( null, config.MyUInt16Nullable );

            Assert.AreEqual<Int32>( -2147483648, config.MyInt32 );
            Assert.AreEqual<Nullable<Int32>>( null, config.MyInt32Nullable );
            Assert.AreEqual<UInt32>( 4294967295, config.MyUInt32 );
            Assert.AreEqual<Nullable<UInt32>>( null, config.MyUInt32Nullable );

            Assert.AreEqual<Int64>( -922337203685477508, config.MyInt64 );
            Assert.AreEqual<Nullable<Int64>>( null, config.MyInt64Nullable );
            Assert.AreEqual<UInt64>( 18446744073709551615, config.MyUInt64 );
            Assert.AreEqual<Nullable<UInt64>>( null, config.MyUInt64Nullable );

            Assert.AreEqual<Single>( (Single)3.402823e38, config.MySingle );
            Assert.AreEqual<Nullable<Single>>( null, config.MySingleNullable );

            Assert.AreEqual<Double>( 1.79769313486231570E+308, config.MyDouble );
            Assert.AreEqual<Nullable<Double>>( null, config.MyDoubleNullable );

            Assert.AreEqual<Char>( (Char)1, config.MyChar );
            Assert.AreEqual<Nullable<Char>>( (Char)0, config.MyCharNullable );

            Assert.AreEqual( true, config.MyBoolean );
            Assert.AreEqual( true, config.MyBooleanNullable );

            Assert.AreEqual<Decimal>( (Decimal).0123456789, config.MyDecimal );
            Assert.AreEqual<Nullable<Decimal>>( null, config.MyDecimalNullable );

            Assert.AreEqual( DateTime.Parse( "2011-05-16 9:42 PM" ), config.MyDateTime );
            Assert.AreEqual( null, config.MyDateTimeNullable );

            Assert.AreEqual( AnEnum.One, config.MyEnum );
            Assert.AreEqual( null, config.MyEnumNullable );
        }
    }

    public class MySimpleConfig
    {
        [ConfigurableOption( "X" )]
        public string Mode { get; set; }
        [ConfigurableOption( "Y" )]
        public string Path { get; set; }  
    }

    public class MyBooleanConfig
    {
        public bool Active{ get; set; }
    }

    public enum AnEnum
    {
        Zero,
        One,
        Two
    }

    public class MyEnumConfig
    {
        public AnEnum Enum { get; set; }
    }

    public class MyComplexConfig
    {
        [ConfigurableOption( "mby" )]
        public Byte MyByte { get; set; }
        [ConfigurableOption( "mbyn" )]
        public Byte? MyByteNullable { get; set; }

        [ConfigurableOption( "msby" )]
        public SByte MySByte { get; set; }
        [ConfigurableOption( "msbyn" )]
        public SByte? MySByteNullable { get; set; }
        
        [ConfigurableOption( "mi16" )]
        public Int16 MyInt16 { get; set; }
        [ConfigurableOption( "mi16n" )]
        public Int16? MyInt16Nullable { get; set; }
        [ConfigurableOption( "mui16" )]
        public UInt16 MyUInt16 { get; set; }
        [ConfigurableOption( "mui16n" )]
        public UInt16? MyUInt16Nullable { get; set; }

        [ConfigurableOption( "mi32" )]
        public Int32 MyInt32 { get; set; }
        [ConfigurableOption( "mi32n" )]
        public Int32? MyInt32Nullable { get; set; }
        [ConfigurableOption( "mui32" )]
        public UInt32 MyUInt32 { get; set; }
        [ConfigurableOption( "mui32n" )]
        public UInt32? MyUInt32Nullable { get; set; }
        
        [ConfigurableOption( "mi64" )]
        public Int64 MyInt64 { get; set; }
        [ConfigurableOption( "mi64n" )]
        public Int64? MyInt64Nullable { get; set; }
        [ConfigurableOption( "mui64" )]
        public UInt64 MyUInt64 { get; set; }
        [ConfigurableOption( "mui64n" )]
        public UInt64? MyUInt64Nullable { get; set; }

        [ConfigurableOption( "ms" )]
        public Single MySingle { get; set; }
        [ConfigurableOption( "msn" )]
        public Single? MySingleNullable { get; set; }

        [ConfigurableOption( "md" )]
        public Double MyDouble { get; set; }
        [ConfigurableOption( "mdn" )]
        public Double? MyDoubleNullable { get; set; }

        [ConfigurableOption( "mc" )]
        public Char MyChar { get; set; }
        [ConfigurableOption( "mcn" )]
        public Char MyCharNullable { get; set; }
        
        [ConfigurableOption( "mb" )]
        public Boolean MyBoolean { get; set; }
        [ConfigurableOption( "mbn" )]
        public Boolean? MyBooleanNullable { get; set; }

        [ConfigurableOption( "mde" )]
        public Decimal MyDecimal { get; set; }
        [ConfigurableOption( "mden" )]
        public Decimal? MyDecimalNullable { get; set; }

        [ConfigurableOption( "mdt" )]
        public DateTime MyDateTime { get; set; }
        [ConfigurableOption( "mdtn" )]
        public DateTime? MyDateTimeNullable { get; set; }

        [ConfigurableOption( "me" )]
        public AnEnum MyEnum { get; set; }
        [ConfigurableOption( "men" )]
        public AnEnum? MyEnumNullable { get; set; }

        [ConfigurableOption( "mst" )]
        public String MyString { get; set; }
    }
}
