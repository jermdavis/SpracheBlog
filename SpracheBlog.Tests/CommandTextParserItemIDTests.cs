using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class CommandTextParserItemIDTests
    {
        [TestMethod]
        public void ItemIDParsesValidID()
        {
            string guid = "{582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d}";
            var result = CommandTextParser.ItemId.TryParse(guid);

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual(Guid.Parse(guid), result.Value.Id);
            Assert.AreEqual(string.Empty, result.Value.Path);
        }

        [TestMethod]
        public void ItemIDFailsForInvalidID()
        {
            string guid = "{582ccf36!b6e4-49f0-9c35-2d8e40b5ef3d}";
            var result = CommandTextParser.ItemId.TryParse(guid);

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void ItemPathParsesForValidPath()
        {
            string path = @"\alpha\bravo";

            var result = CommandTextParser.ItemPath.TryParse(path);

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual(path.Replace('\\', '/'), result.Value.Path);
            Assert.AreEqual(Guid.Empty, result.Value.Id);
        }

        [TestMethod]
        public void ItemPathParsesForValidPathWithTrailingSlash()
        {
            string path = @"/alpha/bravo";

            var result = CommandTextParser.ItemPath.TryParse(path + "\\");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual(path, result.Value.Path);
            Assert.AreEqual(Guid.Empty, result.Value.Id);
        }

        [TestMethod]
        public void ItemPathFailsForInvalidPath()
        {
            string path = @"\alpha!bravo ";

            // Not sure if this is a hack or not - ItemPath only fails to parse the string above
            // if some other parseable data follows after the bad path - like the whitespace here.
            // Without the Then() clause the parse would work, but not consume the "!bravo" bit 
            // which is supposed to be the error...
            var result = CommandTextParser.ItemPath
                .Then(_ => Parse.WhiteSpace)
                .TryParse(path);

            Assert.IsFalse(result.WasSuccessful);
        }
    }


}