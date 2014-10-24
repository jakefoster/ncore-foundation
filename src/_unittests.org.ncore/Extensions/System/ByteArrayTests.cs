/*
Copyright (c) 2004-2010, Cleverbox Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without 
modification, are permitted provided that the following conditions are met: 

- Redistributions of source code must retain the above copyright notice, this 
  list of conditions and the following disclaimer.
	
- Redistributions in binary form must reproduce the above copyright notice, 
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution. 

- Neither the name of NCore.org nor the names of its contributors may be used 
  to endorse or promote products derived from this software without specific 
  prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE 
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Extensions;
using org.ncore.Common;

namespace _unittests.org.ncore.Extensions
{
    /// <summary>
    /// Summary description for ByteArrayTests
    /// </summary>
    [TestClass]
    public class System_ByteArrayTests
    {
        public System_ByteArrayTests()
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
        public void ToText_WithEncoding()
        {
            byte[] input = new byte[] { 98, 121, 116, 101, 32, 97, 114, 114, 97, 121 };
            string result = input.ToText( Encoding.UTF8 );
            string expected = "byte array";

            Assert.AreEqual( expected, result );
        }

        [TestMethod]
        public void ToText()
        {
            byte[] input = new byte[] { 98, 0, 121, 0, 116, 0, 101, 0, 32, 0, 97, 0, 114, 0, 114, 0, 97, 0, 121, 0 };
            string result = input.ToText();
            string expected = "byte array";

            Assert.AreEqual( expected, result );
        }

        [TestMethod]
        public void ToText_OnZeroLengthByteArray()
        {
            byte[] input = new byte[] { };
            string result = input.ToText();
            string expected = string.Empty;

            Assert.AreEqual( expected, result );
        }

        [TestMethod]
        public void ToText_WithStartIndexSpecified()
        {
            byte[] input = new byte[] { 98, 0, 121, 0, 116, 0, 101, 0, 32, 0, 97, 0, 114, 0, 114, 0, 97, 0, 121, 0 };
            string result = input.ToText( 4 );
            string expected = "te array";

            Assert.AreEqual( expected, result );
        }

        [TestMethod]
        public void ToText_WithStartIndexSpecifiedTooBig()
        {
            byte[] input = new byte[] { 98, 0, 121, 0, 116, 0, 101, 0, 32, 0, 97, 0, 114, 0, 114, 0, 97, 0, 121, 0 };
            string result = input.ToText( 20 );
            string expected = string.Empty;

            Assert.AreEqual( expected, result );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        public void ToText_WithStartIndexSpecifiedAsGreaterThanSizeOfByteArray()
        {
            byte[] input = new byte[] { 98, 0, 121, 0, 116, 0, 101, 0, 32, 0, 97, 0, 114, 0, 114, 0, 97, 0, 121, 0 };
            string result = input.ToText( 100 );
        }
    }
}
