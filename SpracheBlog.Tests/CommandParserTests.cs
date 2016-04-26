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
            var result = Command.CreateCommand.TryParse("create {123} named item under /123/234");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as CreateCommand;

            Assert.AreEqual("/123/234", cmd.Location);
            Assert.AreEqual("{123}", cmd.Template);
            Assert.AreEqual(0, cmd.Fields.Count());
        }

        [TestMethod]
        public void CreateWithNameParses()
        {
            var result = Command.CreateCommand.TryParse("create {123} named fred under /123/234 with alpha=\"one\", beta=\"two\"");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as CreateCommand;

            Assert.AreEqual("/123/234", cmd.Location);
            Assert.AreEqual("{123}", cmd.Template);
            Assert.AreEqual(2, cmd.Fields.Count());
            Assert.AreEqual("alpha", cmd.Fields.First().Name);
        }

        [TestMethod]
        public void CreateWithInvalidNameFails()
        {
            var result = Command.CreateCommand.TryParse("create {123} named fr!d under /123/234 with alpha=\"one\", beta=\"two\"");

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void MoveParses()
        {
            var result = Command.MoveCommand.TryParse(@"move \abc\def to {456}");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as MoveCommand;

            Assert.AreEqual("\\abc\\def", cmd.Item);
            Assert.AreEqual("{456}", cmd.NewLocation);
        }

        [TestMethod]
        public void BadMoveDoesNotParse()
        {
            var result = Command.MoveCommand.TryParse(@"move \abc\def ao {456}");

            Assert.IsFalse(result.WasSuccessful);
        }

        [TestMethod]
        public void DeleteParses()
        {
            var result = Command.DeleteCommand.TryParse(@"delete \abc\def\x");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as DeleteCommand;

            Assert.AreEqual("\\abc\\def\\x", cmd.Item);
        }

        [TestMethod]
        public void AnyParsesCreate()
        {
            var result = Command.Any.TryParse("create \\12 named alpha under {xyz} with a=\"2\"");

            Assert.IsTrue(result.WasSuccessful, result.Message);
            Assert.IsInstanceOfType(result.Value, typeof(CreateCommand));

            var cmd = result.Value as CreateCommand;

            Assert.AreEqual("\\12", cmd.Template);
            Assert.AreEqual("{xyz}", cmd.Location);
            Assert.AreEqual("alpha", cmd.Name);
            Assert.AreEqual(1, cmd.Fields.Count());
            Assert.AreEqual("a", cmd.Fields.First().Name);
        }
    }

}