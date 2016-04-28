using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class CommandPathOrIDTests
    {
        [TestMethod]
        public void PathOrIDParsesPath()
        {
            var result = CommandParser.PathOrID.TryParse(" \\test\\path  ");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual("/test/path", result.Value.Path);
            Assert.AreEqual(Guid.Empty, result.Value.Id);
        }

        [TestMethod]
        public void PathOrIDParsesID()
        {
            string id = "{582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d}";
            var result = CommandParser.PathOrID.TryParse(id);

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual(Guid.Parse(id), result.Value.Id);
            Assert.AreEqual(string.Empty, result.Value.Path);
        }
    }

}