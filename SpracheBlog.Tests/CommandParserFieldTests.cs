using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class CommandParserFieldTests
    {
        [TestMethod]
        public void ValidFieldParses()
        {
            var result = CommandParser.Field.TryParse("alpha=\"one\"");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual("alpha", result.Value.Name);
            Assert.AreEqual("one", result.Value.Value);
        }

        [TestMethod]
        public void ValidFieldWithWhitespaceParses()
        {
            var result = CommandParser.Field.TryParse("alpha = \"one\"");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual("alpha", result.Value.Name);
            Assert.AreEqual("one", result.Value.Value);
        }

        [TestMethod]
        public void FieldWithMissingQuoteFails()
        {
            var result = CommandParser.Field.TryParse("alpha=\"one");

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void FieldWithQuotedNameParses()
        {
            var result = CommandParser.Field.TryParse("\"Display name\"=\"one\"");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual("Display name", result.Value.Name);
            Assert.AreEqual("one", result.Value.Value);
        }
    }

}