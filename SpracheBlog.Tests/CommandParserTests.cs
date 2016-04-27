using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpracheBlog;
using Sprache;
using System.Linq;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class CommandParserTests
    {
        [TestMethod]
        public void ItemIDParsesValidID()
        {
            string guid = "{582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d}";
            var result = Command.ItemId.TryParse(guid);

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual(Guid.Parse(guid), result.Value.Id);
            Assert.AreEqual(string.Empty, result.Value.Path);
        }

        [TestMethod]
        public void ItemIDFailsForInvalidID()
        {
            string guid = "{582ccf36!b6e4-49f0-9c35-2d8e40b5ef3d}";
            var result = Command.ItemId.TryParse(guid);

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void ItemPathParsesForValidPath()
        {
            string path = @"\alpha\bravo";

            var result = Command.ItemPath.TryParse(path);

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual(path.Replace('\\','/'), result.Value.Path);
            Assert.AreEqual(Guid.Empty, result.Value.Id);
        }

        [TestMethod]
        public void ItemPathParsesForValidPathWithTrailingSlash()
        {
            string path = @"/alpha/bravo";

            var result = Command.ItemPath.TryParse(path + "\\");

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
            var result = Command.ItemPath
                .Then(_ => Parse.WhiteSpace)
                .TryParse(path);

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void PathOrIDParsesPath()
        {
            var result = Command.PathOrID.TryParse(" \\test\\path  ");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual("/test/path", result.Value.Path);
            Assert.AreEqual(Guid.Empty, result.Value.Id);
        }

        [TestMethod]
        public void PathOrIDParsesID()
        {
            string id = "{582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d}";
            var result = Command.PathOrID.TryParse(id);

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual(Guid.Parse(id), result.Value.Id);
            Assert.AreEqual(string.Empty, result.Value.Path);
        }

        [TestMethod]
        public void ValidNameParses()
        {
            var result = Command.ItemName.TryParse(" Hello ");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual("Hello", result.Value);
        }

        [TestMethod]
        public void ValidFieldParses()
        {
            var result = Command.Field.TryParse("alpha=\"one\"");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual("alpha", result.Value.Name);
            Assert.AreEqual("one", result.Value.Value);
        }

        [TestMethod]
        public void ValidFieldWithWhitespaceParses()
        {
            var result = Command.Field.TryParse("alpha = \"one\"");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.AreEqual("alpha", result.Value.Name);
            Assert.AreEqual("one", result.Value.Value);
        }

        [TestMethod]
        public void FieldWithMissingQuoteFails()
        {
            var result = Command.Field.TryParse("alpha=\"one");

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void CreateWithoutWithParses()
        {
            var result = Command.CreateCommand.TryParse("create {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d} named item under /123/234");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as CreateCommand;

            Assert.AreEqual("/123/234", cmd.Location.Path);
            Assert.AreEqual(Guid.Parse("582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d"), cmd.Template.Id);
            Assert.AreEqual(0, cmd.Fields.Count());
        }

        [TestMethod]
        public void CreateWithNameParses()
        {
            var result = Command.CreateCommand.TryParse("create {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d} named fred under /123/234 with alpha=\"one\", beta=\"two\"");

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
            var result = Command.CreateCommand.TryParse("create {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d} named fr!d under /123/234 with alpha=\"one\", beta=\"two\"");

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void MoveParses()
        {
            var result = Command.MoveCommand.TryParse(@"move \abc\def to {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d}");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as MoveCommand;

            Assert.AreEqual("/abc/def", cmd.Item.Path);
            Assert.AreEqual(Guid.Parse("582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d"), cmd.NewLocation.Id);
        }

        [TestMethod]
        public void BadMoveDoesNotParse()
        {
            var result = Command.MoveCommand.TryParse(@"move \abc\def ao {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d}");

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void MoveFailsForInvalidPath()
        {
            var result = Command.MoveCommand.TryParse(@"move \abc!def to {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d}");

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void DeleteParses()
        {
            var result = Command.DeleteCommand.TryParse(@"delete \abc\def\x");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as DeleteCommand;

            Assert.AreEqual("/abc/def/x", cmd.Item.Path);
        }

        [TestMethod]
        public void AnyCanParseCreate()
        {
            var result = Command.Any.TryParse("create \\12 named alpha under {582ccf36-b6e4-49f0-9c35-2d8e40b5ef3d} with a=\"2\"");

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
            var result = Command.Any.TryParse("MOVE {cf2d0f82-8504-4b7e-a8c4-60658be8688b} to /sitecore/content\\home");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.IsInstanceOfType(result.Value, typeof(MoveCommand));

            var cmd = result.Value as MoveCommand;
            Assert.AreEqual(Guid.Parse("{cf2d0f82-8504-4b7e-a8c4-60658be8688b}"), cmd.Item.Id);
            Assert.AreEqual("/sitecore/content/home", cmd.NewLocation.Path);
        }

        [TestMethod]
        public void AnyCanParseDelete()
        {
            var result = Command.Any.TryParse("DelEte /a\\b/c");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.IsInstanceOfType(result.Value, typeof(DeleteCommand));

            var cmd = result.Value as DeleteCommand;
            Assert.AreEqual("/a/b/c", cmd.Item.Path);
        }
    }

}