using DolDoc.Core.Parser;
using DolDoc.Editor;
using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DolDoc.Tests.Commands
{
    [TestClass]
    public class WriteStringTests
    {
        private Document _document;
        private CommandContext _commandContext;

        [TestInitialize]
        public void Setup()
        {
            _document = new Document();
            _commandContext = new CommandContext
            {
                State = new ViewerState(null, _document, 640, 480)
            };
        }

        [TestMethod]
        public void WritesSimpleString()
        {
            const string str = "Hello World!";

            _document.Load(str);

            var ws = new WriteString();
            var result = ws.Execute(_document.Entries.First(), _commandContext);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(str.Length, result.WrittenCharacters);
            // Assert.AreEqual(str.Length, _commandContext.State.CursorPosition);

            for (var i = 0; i < str.Length; i++)
            {
                Assert.AreEqual(_document.Entries.First(), _commandContext.State.Pages[i].Entry);
                Assert.AreEqual((byte)str[i], _commandContext.State.Pages[i].Char);
            }


        }
    }
}
