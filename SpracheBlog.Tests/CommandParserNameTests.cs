using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class CommandParserNameTests
    {
        [TestMethod]
        public void ValidNameParses()
        {
            var result = CommandParser.ItemName.TryParse(" Hello ");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual("Hello", result.Value);
        }
    }

}