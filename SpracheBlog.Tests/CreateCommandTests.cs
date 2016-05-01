using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.FakeDb;
using Sitecore.Data;
using System.Collections.Generic;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class CreateCommandTests
    {
        [TestMethod]
        public void ValidCreateWithoutFieldsWorks()
        {
            CommandProcessor cp = new CommandProcessor();
            using (Db db = new Db())
            {
                var f1 = new DbItem("Folder1");
                db.Add(f1);

                var templateID = ID.Parse("9b3d796f-a72b-45cd-8122-199beac8aef6");
                var template = new DbTemplate("itemTemplate", templateID);
                db.Add(template);

                Assert.AreEqual(0, f1.Children.Count);

                CreateCommand cmd = new CreateCommand();
                cmd.Template = new ItemIdenitfier() { Id = new Guid("9b3d796f-a72b-45cd-8122-199beac8aef6") };
                cmd.Name = "test";
                cmd.Location = new ItemIdenitfier() { Path = "/sitecore/content/Folder1" };

                string result = cmd.Execute();

                Assert.IsTrue(result.StartsWith("created", StringComparison.InvariantCultureIgnoreCase));
                Assert.AreEqual(1, f1.Children.Count);

                var folder = Sitecore.Context.Database.GetItem("/sitecore/content/Folder1");

                Assert.IsNotNull(folder.Children["test"]);
            }
        }

        [TestMethod]
        public void ValidCreateWithFieldsWorks()
        {
            CommandProcessor cp = new CommandProcessor();
            using (Db db = new Db())
            {
                var f1 = new DbItem("Folder1");
                db.Add(f1);

                var templateID = ID.Parse("9b3d796f-a72b-45cd-8122-199beac8aef6");
                var template = new DbTemplate("itemTemplate", templateID);
                db.Add(template);

                Assert.AreEqual(0, f1.Children.Count);

                CreateCommand cmd = new CreateCommand();
                cmd.Template = new ItemIdenitfier() { Id = new Guid("9b3d796f-a72b-45cd-8122-199beac8aef6") };
                cmd.Name = "test";
                cmd.Location = new ItemIdenitfier() { Path = "/sitecore/content/Folder1" };
                cmd.Fields = new List<Field> { new Field() { Name = "__Display name", Value = "Test Item" } };

                var result = cmd.Execute();

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