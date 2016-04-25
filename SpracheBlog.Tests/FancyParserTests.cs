using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpracheBlog;
using Sprache;

namespace SpracheBlog.Tests
{
    /*
    [TestClass]
    public class FancyParserTests
    {
        [TestMethod]
        public void ParseAValidID()
        {
            string id = "{d457c8e9-4f32-4379-ac99-33317a78de13}";

            var result = FancyCommand.ItemID.TryParse(id);

            Assert.IsTrue(result.WasSuccessful);
            Assert.AreEqual(new Guid(id), result.Value);
        }

        [TestMethod]
        public void ParseAnInvalidID_MissingCurly()
        {
            var result = FancyCommand.ItemID.TryParse("d457c8e9-4f32-4379-ac99-33317a78de13}");

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void ParseAnInvalidID_MissingCharacters()
        {
            var result = FancyCommand.ItemID.TryParse("{d457c8e9-4f324379-ac99-33317a78de13}");

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void ParseAValidPath()
        {
            string path = @"\hello\something\stuff";
            var result = FancyCommand.ItemPath.TryParse(path);

            Assert.IsTrue(result.WasSuccessful);
            Assert.AreEqual(path, result.Value);
        }

        [TestMethod]
        public void ParseAnInvalidPath()
        {
            string path = @"\h!ello\something\stuff";
            var result = FancyCommand.ItemPath.TryParse(path);

            Assert.IsFalse(result.WasSuccessful);
        }
    }
    */
}