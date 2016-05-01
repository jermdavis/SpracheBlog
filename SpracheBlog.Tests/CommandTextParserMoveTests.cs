using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class CommandTextParserMoveTests
    {
        [TestMethod]
        public void MoveParses()
        {
            var result = CommandTextParser.MoveCommand.TryParse(@"move \abc\def to {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d}");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as MoveCommand;

            Assert.AreEqual("/abc/def", cmd.Item.Path);
            Assert.AreEqual(Guid.Parse("582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d"), cmd.NewLocation.Id);
        }

        [TestMethod]
        public void BadMoveDoesNotParse()
        {
            var result = CommandTextParser.MoveCommand.TryParse(@"move \abc\def ao {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d}");

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void MoveFailsForInvalidPath()
        {
            var result = CommandTextParser.MoveCommand.TryParse(@"move \abc!def to {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d}");

            Assert.IsFalse(result.WasSuccessful);
        }
    }

}