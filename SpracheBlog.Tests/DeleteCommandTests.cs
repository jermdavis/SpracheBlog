using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.FakeDb;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class DeleteCommandTests
    {
        [TestMethod]
        public void ValidDeleteWorks()
        {
            CommandProcessor cp = new CommandProcessor();
            using (Db db = new Db())
            {
                var f1 = new DbItem("Folder1");
                var p1 = new DbItem("Page");
                f1.Add(p1);
                db.Add(f1);

                Assert.AreEqual(1, f1.Children.Count);

                DeleteCommand cmd = new DeleteCommand();
                cmd.Item = new ItemIdenitfier() { Path="/sitecore/content/Folder1/Page" };

                var result = cmd.Execute();

                Assert.IsTrue(result.StartsWith("deleted", StringComparison.InvariantCultureIgnoreCase));
                Assert.AreEqual(0, f1.Children.Count);
            }
        }
    }

}
