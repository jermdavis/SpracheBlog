﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;

namespace SpracheBlog.Tests
{

    [TestClass]
    public class CommandParserDeleteTests
    {
        [TestMethod]
        public void DeleteParses()
        {
            var result = CommandParser.DeleteCommand.TryParse(@"delete \abc\def\x");

            Assert.IsTrue(result.WasSuccessful, result.Message);

            var cmd = result.Value as DeleteCommand;

            Assert.AreEqual("/abc/def/x", cmd.Item.Path);
        }
    }

}