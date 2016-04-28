using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;
using System.Linq;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class CommandParserCreateTests
    {
        [TestMethod]
        public void CreateWithoutWithParses()
        {
            var result = CommandParser.CreateCommand.TryParse("create {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d} named item under /123/234");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as CreateCommand;

            Assert.AreEqual("/123/234", cmd.Location.Path);
            Assert.AreEqual(Guid.Parse("582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d"), cmd.Template.Id);
            Assert.AreEqual(0, cmd.Fields.Count());
        }

        [TestMethod]
        public void CreateWithNameParses()
        {
            var result = CommandParser.CreateCommand.TryParse("create {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d} named fred under /123/234 with alpha=\"one\", beta=\"two\"");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as CreateCommand;

            Assert.AreEqual("/123/234", cmd.Location.Path);
            Assert.AreEqual(Guid.Parse("582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d"), cmd.Template.Id);
            Assert.AreEqual(2, cmd.Fields.Count());
            Assert.AreEqual("alpha", cmd.Fields.First().Name);
        }

        [TestMethod]
        public void CreateWithInvalidNameFails()
        {
            var result = CommandParser.CreateCommand.TryParse("create {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d} named fr!d under /123/234 with alpha=\"one\", beta=\"two\"");

            Assert.IsFalse(result.WasSuccessful);
        }
    }

}