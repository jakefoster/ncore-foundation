using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Common;
using org.ncore.Diagnostics;

namespace _unittests.org.ncore.Common
{
    /// <summary>
    /// Summary description for MemberMapperTests
    /// </summary>
    [TestClass]
    public class MemberMapperTests
    {
        private const int MAPPABLE_INT = 654321;
        private const string MAPPABLE_STRING = "MappableString";
        private const int UNMATCHED_INT = 123456;

        public MemberMapperTests()
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
        public void Map_ObjectToObject()
        {
            SourceObject source = new SourceObject();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            TargetObject target = new TargetObject();
            target = (TargetObject)MemberMapper.Map( source, target );

            Assert.AreEqual( source.MappableInt, target.MappableInt );
            Assert.AreEqual( source.MappableString, target.MappableString );
            Assert.AreEqual( source.ReadOnlyString, target.ReadOnlyString );
        }

        [TestMethod]
        public void Map_ObjectToObject_Properties()
        {
            SourceObject source = new SourceObject();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            TargetObject target = new TargetObject();
            target = (TargetObject)MemberMapper.Map( source, target, MemberMapper.MemberTypeEnum.Property );

            Assert.AreEqual( source.MappableInt, target.MappableInt );
            Assert.AreEqual( source.MappableString, target.MappableString );
            Assert.AreEqual( string.Empty, target.ReadOnlyString );
        }

        [TestMethod]
        public void Map_ObjectToStruct()
        {
            SourceObject source = new SourceObject();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            TargetStruct target = new TargetStruct();
            target = (TargetStruct)MemberMapper.Map( source, target );

            Assert.AreEqual( source.MappableInt, target.MappableInt );
            Assert.AreEqual( source.MappableString, target.MappableString );
            Assert.AreEqual( source.ReadOnlyString, target.ReadOnlyString );
        }

        [TestMethod]
        public void Map_ObjectToStruct_Properties()
        {
            SourceObject source = new SourceObject();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            TargetStruct target = new TargetStruct();
            target = (TargetStruct)MemberMapper.Map( source, target, MemberMapper.MemberTypeEnum.Property );

            Assert.AreEqual( source.MappableInt, target.MappableInt );
            Assert.AreEqual( source.MappableString, target.MappableString );
            Assert.IsNull( target.ReadOnlyString );
        }

        [TestMethod]
        public void Map_StructToStruct()
        {
            SourceStruct source = new SourceStruct();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            TargetStruct target = new TargetStruct();
            target = (TargetStruct)MemberMapper.Map( source, target );

            Assert.AreEqual( source.MappableInt, target.MappableInt );
            Assert.AreEqual( source.MappableString, target.MappableString );
            Assert.AreEqual( source.ReadOnlyString, target.ReadOnlyString );
        }

        [TestMethod]
        public void Map_StructToStruct_Properties()
        {
            SourceStruct source = new SourceStruct();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            TargetStruct target = new TargetStruct();
            target = (TargetStruct)MemberMapper.Map( source, target, MemberMapper.MemberTypeEnum.Property );

            Assert.AreEqual( source.MappableInt, target.MappableInt );
            Assert.AreEqual( source.MappableString, target.MappableString );
            Assert.IsNull( target.ReadOnlyString );
        }

        [TestMethod]
        public void Map_StructToObject()
        {
            SourceStruct source = new SourceStruct();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            TargetObject target = new TargetObject();
            target = (TargetObject)MemberMapper.Map( source, target );

            Assert.AreEqual( source.MappableInt, target.MappableInt );
            Assert.AreEqual( source.MappableString, target.MappableString );
            Assert.AreEqual( source.ReadOnlyString, target.ReadOnlyString );
        }

        [TestMethod]
        public void Map_StructToObject_Properties()
        {
            SourceStruct source = new SourceStruct();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            TargetObject target = new TargetObject();
            target = (TargetObject)MemberMapper.Map( source, target, MemberMapper.MemberTypeEnum.Property );

            Assert.AreEqual( source.MappableInt, target.MappableInt );
            Assert.AreEqual( source.MappableString, target.MappableString );
            Assert.AreEqual( string.Empty, target.ReadOnlyString );
        }

        [TestMethod]
        [ExpectedException( typeof( MissingTargetMemberException ) )]
        public void Map_ObjectToObjectWithMissingMembers()
        {
            SourceObject source = new SourceObject();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            ObjectWithMissingMembers target = new ObjectWithMissingMembers();
            target = (ObjectWithMissingMembers)MemberMapper.Map( source, target, MemberMapper.MemberTypeEnum.Field, false, MemberMapper.MappingTemplateEnum.Source );
        }

        [TestMethod]
        [ExpectedException( typeof( MissingTargetMemberException ) )]
        public void Map_ObjectToObjectWithMissingMembers_Properties()
        {
            SourceObject source = new SourceObject();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            ObjectWithMissingMembers target = new ObjectWithMissingMembers();
            target = (ObjectWithMissingMembers)MemberMapper.Map( source, target, MemberMapper.MemberTypeEnum.Property, false, MemberMapper.MappingTemplateEnum.Source );
        }

        [TestMethod]
        [ExpectedException( typeof( MissingSourceMemberException ) )]
        public void Map_ObjectWithMissingMembersToObject()
        {
            ObjectWithMissingMembers source = new ObjectWithMissingMembers();
            source.MappableInt = MAPPABLE_INT;

            TargetObject target = new TargetObject();
            target = (TargetObject)MemberMapper.Map( source, target, MemberMapper.MemberTypeEnum.Field, false, MemberMapper.MappingTemplateEnum.Target );
        }

        [TestMethod]
        [ExpectedException( typeof( MissingSourceMemberException ) )]
        public void Map_ObjectWithMissingMembersToObject_Properties()
        {
            ObjectWithMissingMembers source = new ObjectWithMissingMembers();
            source.MappableInt = MAPPABLE_INT;

            TargetObject target = new TargetObject();
            target = (TargetObject)MemberMapper.Map( source, target, MemberMapper.MemberTypeEnum.Property, false, MemberMapper.MappingTemplateEnum.Target );
        }

        [TestMethod]
        [ExpectedException( typeof( MemberDataTypeMissmatchException ) )]
        public void Map_ObjectToObjectWithWrongDataType()
        {
            SourceObject source = new SourceObject();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            ObjectWithWrongDataType target = new ObjectWithWrongDataType();
            target = (ObjectWithWrongDataType)MemberMapper.Map( source, target );
        }

        [TestMethod]
        [ExpectedException( typeof( MemberDataTypeMissmatchException ) )]
        public void Map_ObjectToObjectWithWrongDataType_Properties()
        {
            SourceObject source = new SourceObject();
            source.MappableInt = MAPPABLE_INT;
            source.MappableString = MAPPABLE_STRING;
            source.UnmatchedInt = UNMATCHED_INT;

            ObjectWithWrongDataType target = new ObjectWithWrongDataType();
            target = (ObjectWithWrongDataType)MemberMapper.Map( source, target, MemberMapper.MemberTypeEnum.Property );
        }
    }


    internal class SourceObject
    {
        private int _mappableInt = 0;
        private string _mappableString = string.Empty;
        private readonly string _readOnlyString = "ReadOnlyString";
        private int _unmatchedInt = 0;

        public int MappableInt
        {
            get { return _mappableInt; }
            set { _mappableInt = value; }
        }

        public string MappableString
        {
            get { return _mappableString; }
            set { _mappableString = value; }
        }

        public string ReadOnlyString
        {
            get { return _readOnlyString; }
        }

        public string WriteOnlyString
        {
            set { return; }
        }

        public int UnmatchedInt
        {
            get { return _unmatchedInt; }
            set { _unmatchedInt = value; }
        }
    }

    internal class TargetObject
    {
        private int _mappableInt = 0;
        private string _mappableString = string.Empty;
        private readonly string _readOnlyString = string.Empty;

        public int MappableInt
        {
            get { return _mappableInt; }
            set { _mappableInt = value; }
        }

        public string MappableString
        {
            get { return _mappableString; }
            set { _mappableString = value; }
        }

        public string ReadOnlyString
        {
            get { return _readOnlyString; }
        }

        public string WriteOnlyString
        {
            set { return; }
        }
    }

    internal struct SourceStruct
    {
        private int _mappableInt;
        private string _mappableString;
        private readonly string _readOnlyString;
        private int _unmatchedInt;

        public int MappableInt
        {
            get { return _mappableInt; }
            set { _mappableInt = value; }
        }

        public string MappableString
        {
            get { return _mappableString; }
            set { _mappableString = value; }
        }

        public string ReadOnlyString
        {
            get { return _readOnlyString; }
        }

        public string WriteOnlyString
        {
            set { return; }
        }

        public int UnmatchedInt
        {
            get { return _unmatchedInt; }
            set { _unmatchedInt = value; }
        }

        public SourceStruct( string readOnlyString )
        {
            _readOnlyString = readOnlyString;
            _mappableInt = 0;
            _mappableString = string.Empty;
            _unmatchedInt = 0;
        }
    }

    internal struct TargetStruct
    {
        private int _mappableInt;
        private string _mappableString;
        private readonly string _readOnlyString;

        public int MappableInt
        {
            get { return _mappableInt; }
            set { _mappableInt = value; }
        }

        public string MappableString
        {
            get { return _mappableString; }
            set { _mappableString = value; }
        }

        public string ReadOnlyString
        {
            get { return _readOnlyString; }
        }

        public string WriteOnlyString
        {
            set { return; }
        }

        public TargetStruct( string readOnlyString )
        {
            _readOnlyString = readOnlyString;
            _mappableInt = 0;
            _mappableString = string.Empty;
        }
    }

    internal class ObjectWithMissingMembers
    {
        private int _mappableInt = 0;
        private readonly string _readOnlyString = string.Empty;

        public int MappableInt
        {
            get { return _mappableInt; }
            set { _mappableInt = value; }
        }

        public string ReadOnlyString
        {
            get { return _readOnlyString; }
        }

        public string WriteOnlyString
        {
            set { return; }
        }
    }

    internal class ObjectWithWrongDataType
    {
        private string _mappableInt = string.Empty;
        private int _mappableString = 0;

        public string MappableInt
        {
            get { return _mappableInt; }
            set { _mappableInt = value; }
        }

        public int MappableString
        {
            get { return _mappableString; }
            set { _mappableString = value; }
        }
    }
}
