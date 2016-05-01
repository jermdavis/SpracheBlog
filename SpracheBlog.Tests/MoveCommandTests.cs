using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.FakeDb;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class MoveCommandTests
    {
        [TestMethod]
        public void ValidMoveWorks()
        {
            CommandProcessor cp = new CommandProcessor();
            using (Db db = new Db())
            {
                var f1 = new DbItem("Folder1");
                var f2 = new DbItem("Folder2");
                var p1 = new DbItem("Page");
                f1.Add(p1);
                db.Add(f1);
                db.Add(f2);

                Assert.AreEqual(1, f1.Children.Count);
                Assert.AreEqual(0, f2.Children.Count);

                MoveCommand cmd = new MoveCommand();
                cmd.Item = new ItemIdenitfier() { Path = "/sitecore/content/Folder1/Page" };
                cmd.NewLocation = new ItemIdenitfier() { Path = "/sitecore/content/Folder2" };

                var result = cmd.Execute();

                Assert.IsTrue(result.StartsWith("moved", StringComparison.InvariantCultureIgnoreCase));
                Assert.AreEqual(0, f1.Children.Count);
                Assert.AreEqual(1, f2.Children.Count);
            }
        }
    }

}