using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.FakeDb;
using Sitecore.Data;

namespace SpracheBlog.Tests
{
    [TestClass]
    public class CommandProcessorTests
    {

        [TestMethod]
        public void ExecutingEmptyStringDoesNothing()
        {
            CommandProcessor cp = new CommandProcessor();
            var result = cp.Run("");

            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void ExecutingDeleteWorks()
        {
            CommandProcessor cp = new CommandProcessor();
            using (Db db = new Db())
            {
                var f1 = new DbItem("Folder1");
                var p1 = new DbItem("Page");
                f1.Add(p1);
                db.Add(f1);

                string cmd = "Delete /sitecore/content/Folder1/Page";

                Assert.AreEqual(1, f1.Children.Count);

                var result = cp.Run(cmd);

                Assert.IsTrue(result.StartsWith("deleted", StringComparison.InvariantCultureIgnoreCase));
                Assert.AreEqual(0, f1.Children.Count);
            }
        }

        [TestMethod]
        public void ExecutingMoveWorks()
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

                string cmd = "Move /sitecore/content/Folder1/Page to /sitecore/content/Folder2";

                Assert.AreEqual(1, f1.Children.Count);
                Assert.AreEqual(0, f2.Children.Count);

                var result = cp.Run(cmd);

                Assert.IsTrue(result.StartsWith("moved", StringComparison.InvariantCultureIgnoreCase));
                Assert.AreEqual(0, f1.Children.Count);
                Assert.AreEqual(1, f2.Children.Count);
            }
        }

        [TestMethod]
        public void ExecutingCreateWorks()
        {
            CommandProcessor cp = new CommandProcessor();
            using (Db db = new Db())
            {
                var f1 = new DbItem("Folder1");
                db.Add(f1);

                var templateID = ID.Parse("9b3d796f-a72b-45cd-8122-199beac8aef6");
                var template = new DbTemplate("itemTemplate", templateID);
                db.Add(template);

                string cmd = "create " + templateID.ToString() + " named test under /sitecore/content/Folder1";

                Assert.AreEqual(0, f1.Children.Count);

                var result = cp.Run(cmd);

                Assert.IsTrue(result.StartsWith("created", StringComparison.InvariantCultureIgnoreCase));
                Assert.AreEqual(1, f1.Children.Count);

                var folder = Sitecore.Context.Database.GetItem("/sitecore/content/Folder1");

                Assert.IsNotNull(folder.Children["test"]);
            }
        }

        [TestMethod]
        public void ExecutingCreateWithFieldsWorks()
        {
            CommandProcessor cp = new CommandProcessor();
            using (Db db = new Db())
            {
                var f1 = new DbItem("Folder1");
                db.Add(f1);

                var templateID = ID.Parse("9b3d796f-a72b-45cd-8122-199beac8aef6");
                var template = new DbTemplate("itemTemplate", templateID);
                db.Add(template);

                string cmd = "create " + templateID.ToString() + " named test under /sitecore/content/Folder1 with \"__Display name\"=\"Test Item\"";

                Assert.AreEqual(0, f1.Children.Count);

                var result = cp.Run(cmd);

                Assert.IsTrue(result.StartsWith("created", StringComparison.InvariantCultureIgnoreCase));
                Assert.AreEqual(1, f1.Children.Count);

                var folder = Sitecore.Context.Database.GetItem("/sitecore/content/Folder1");
                var item = folder.Children["test"];

                Assert.IsNotNull(item);
                Assert.AreEqual("Test Item", item.DisplayName);
            }
        }

    }
}
