using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Store
    {
        [TestMethod]
        public void FileNameDates()
        {
            DateTime mydate = new DateTime(2013, 10, 25);

            string expected = "\"25-Oct-2013\"";
            string actual = String.Format("\"{0}\"", mydate.ToString("dd-MMM-yyyy"));

            Assert.AreEqual(expected,actual);
        }
    }
}
