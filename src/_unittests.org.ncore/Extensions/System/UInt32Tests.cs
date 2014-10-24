using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Diagnostics;
using org.ncore.Extensions;

namespace _unittests.org.ncore.Extensions
{
    [TestClass]
    public class System_UInt32Tests
    {
        [TestMethod]
        public void ToBitArray_MinValue()
        {
            UInt32 source = UInt32.MinValue;
            bool[] bits = source.ToBitArray();

            bool[] expected = new bool[ 32 ];

            Assert.IsTrue( expected.SequenceEqual( bits ) );
        }

        [TestMethod]
        public void ToBitArray_Simple9()
        {
            UInt32 source = 9;
            bool[] bits = source.ToBitArray();

            bool[] expected = new bool[ 32 ];
            expected.SetBit( 1, true );
            expected.SetBit( 8, true );

            Assert.IsTrue( expected.SequenceEqual( bits ) );
        }

        [TestMethod]
        public void ToBitArray_MaxValue()
        {
            UInt32 source = UInt32.MaxValue;
            bool[] bits = source.ToBitArray();

            bool[] expected = new bool[ 32 ].Fill( true );

            Assert.IsTrue( expected.SequenceEqual( bits ) );
        }
    }
}
