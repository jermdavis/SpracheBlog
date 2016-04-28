using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.FakeDb;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class Does_FakeDB_Work
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new Db
                    {
                      new DbItem("Home") { { "Title", "Welcome!" } }
                    })
            {

                var home = Sitecore.Context.Database.GetItem("/sitecore/content/home");

                Assert.AreEqual("Welcome!", home["Title"]);
            }
        }

    }

}