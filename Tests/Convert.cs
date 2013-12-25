using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HostFeed;
using ConvertHelper;
using System.IO;
using System.Security.Principal;
using System.Configuration;

namespace Tests
{
    /// <summary>
    /// Summary description for Convert
    /// </summary>
    [TestClass]
    public class Convert
    {
        public Convert()
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
        [TestCategory("Debug")]
        //Easier that debugging !!!!. 
        //Needs to be run with elevated permissions and app.config needs to be populated properly
        public void ConvertTest()
        {
            if (CheckRunAsAdministrator() && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ToEmail"]))
            {
                RssFeedHost host = new RssFeedHost();

                host.Start();

                List<HTMLPage> webPages = FileHandler.GetFiles(ConfigurationManager.AppSettings["StoreDirectory"]);

                CreateRSSData.CreateRSSItems(webPages);

                Converter.ConvertUsingExternalTool();

                host.Stop();
            }
            else
            {
                throw new Exception("Needs to be run with elevated permissions and app.config needs to be populated properly");
            }
        }

        internal static bool CheckRunAsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
