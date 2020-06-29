using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Tests.Commands
{
    [TestClass]
    public class BlinkTests
    {
        [TestMethod]
        public void EnablesBlinkMode()
        {
            var entry = new DocumentEntry(DocumentCommand.Blink, 0, null, new[] { new Argument(null, "1") });
            var cmd = new Blink();
            var ctx = new CommandContext { Blink = false };

            cmd.Execute(entry, ctx);

            Assert.AreEqual(true, ctx.Blink);
        }

        [TestMethod]
        public void DisablesBlinkMode()
        {
            var entry = new DocumentEntry(DocumentCommand.Blink, 0, null, new[] { new Argument(null, "0") });
            var cmd = new Blink();
            var ctx = new CommandContext { Blink = true };

            cmd.Execute(entry, ctx);

            Assert.AreEqual(false, ctx.Blink);
        }
    }
}
