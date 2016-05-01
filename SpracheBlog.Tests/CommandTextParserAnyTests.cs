using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpracheBlog;
using Sprache;
using System.Linq;

namespace SpracheBlog.Tests
{
    
    [TestClass]
    public class CommandTextParserAnyTests
    {
        [TestMethod]
        public void AnyCanParseCreate()
        {
            var result = CommandTextParser.Any.TryParse("create \\12 named alpha under {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d} with a=\"2\"");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.IsInstanceOfType(result.Value, typeof(CreateCommand));

            var cmd = result.Value as CreateCommand;

            Assert.AreEqual("/12", cmd.Template.Path);
            Assert.AreEqual(Guid.Parse("582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d"), cmd.Location.Id);
            Assert.AreEqual("alpha", cmd.Name);
            Assert.AreEqual(1, cmd.Fields.Count());
            Assert.AreEqual("a", cmd.Fields.First().Name);
        }

        [TestMethod]
        public void AnyCanParseMove()
        {
            var result = CommandTextParser.Any.TryParse("MOVE {cf2d0f82-8504-4b7e-a8c4-60658be8688b} to /sitecore/content\\home");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.IsInstanceOfType(result.Value, typeof(MoveCommand));

            var cmd = result.Value as MoveCommand;
            Assert.AreEqual(Guid.Parse("{cf2d0f82-8504-4b7e-a8c4-60658be8688b}"), cmd.Item.Id);
            Assert.AreEqual("/sitecore/content/home", cmd.NewLocation.Path);
        }

        [TestMethod]
        public void AnyCanParseDelete()
        {
            var result = CommandTextParser.Any.TryParse("DelEte /a\\b/c");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.IsInstanceOfType(result.Value, typeof(DeleteCommand));

            var cmd = result.Value as DeleteCommand;
            Assert.AreEqual("/a/b/c", cmd.Item.Path);
        }
    }

}