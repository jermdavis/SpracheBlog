using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class CommandTextParserDeleteTests
    {
        [TestMethod]
        public void DeleteParsesWithPath()
        {
            var result = CommandTextParser.DeleteCommand.TryParse(@"delete \abc\def\x");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as DeleteCommand;

            Assert.AreEqual("/abc/def/x", cmd.Item.Path);
        }

        [TestMethod]
        public void DeleteParsesWithID()
        {
            var result = CommandTextParser.DeleteCommand.TryParse(@"delete {7f700de5-9f24-4a30-a4f7-ed3421abb563}");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as DeleteCommand;

            Assert.AreEqual(Guid.Parse("{7f700de5-9f24-4a30-a4f7-ed3421abb563}"), cmd.Item.Id);
        }
    }

}