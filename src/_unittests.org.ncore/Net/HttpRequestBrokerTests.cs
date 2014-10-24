// TODO: These tests are *really* questionable.  Admittedly, this is a tough one to test but we need to figure something out.  JF

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Diagnostics;
using org.ncore.Net;
using _unittests.org.ncore;

namespace _unittests.org.ncore.Net
{
    /// <summary>
    /// Summary description for HttpRequestBrokerTests
    /// </summary>
    [TestClass]
    public class HttpRequestBrokerTests
    {
        public HttpRequestBrokerTests()
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

        // TODO: Real tests needed!
        [TestMethod]
        public void RunSync()
        {
            HttpRequestBroker requestBroker = new HttpRequestBroker( "http://www.google.com", string.Empty );
            requestBroker.Method = HttpBackgroundWorker.Method.GET;       
            requestBroker.ContentType = "expression/xml; charset=utf-8";
            requestBroker.Headers.Add("Foo", "Bar");

            HttpBackgroundWorker backgroundWorker = new HttpBackgroundWorker( requestBroker );

            backgroundWorker.Run();
            string response = backgroundWorker.ResponseBroker.GetBody();
        }

        // TODO: This test is not going to do us any good because the thread gets killed before the response can be returned.
        //  (It's async after all - duh)  So we need something that spawns a thread and hangs around waiting for it.  JF
        [TestMethod]
        public void RunAsync()
        {
            try
            {
                StringBuilder trace = new StringBuilder();
                UnitTestTraceListener listener = new UnitTestTraceListener( trace );
                Trace.Listeners.Add( listener );

                HttpRequestBroker requestBroker = new HttpRequestBroker( "http://www.google.com", string.Empty );
                requestBroker.Method = HttpBackgroundWorker.Method.GET;
                requestBroker.ContentType = "expression/xml; charset=utf-8";
                requestBroker.Headers.Add( "Foo", "Bar" );
                
                HttpBackgroundWorker backgroundWorker = new HttpBackgroundWorker( requestBroker );

                backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler( backgroundWorker_RunWorkerCompleted );
                backgroundWorker.ProgressChanged += new ProgressChangedEventHandler( backgroundWorker_ProgressChanged );
                backgroundWorker.RunWorkerAsync();

                Spy.Trace( "Called it!" );

                Trace.Write( trace.ToString() );
            }
            catch( Exception ex )
            {
                Spy.Trace( ex );
            }
        }

        public void backgroundWorker_ProgressChanged( object sender, ProgressChangedEventArgs e )
        {
            Spy.Trace( e.ProgressPercentage.ToString() );
        }

        public void backgroundWorker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            Spy.Trace( "In backgroundWorker_RunWorkerCompleted()" );
            Spy.Trace( e.Result.ToString() );
            Spy.Trace( ( (HttpBackgroundWorker)sender ).ResponseBroker.GetBody() );
        }
    }
}
