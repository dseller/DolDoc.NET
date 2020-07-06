using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DolDoc.Tests.Commands
{
    [TestClass]
    public class BlinkTests
    {
        [TestMethod]
        public void EnablesBlinkMode()
        {
            var entry = new Blink(null, new[] { new Argument(null, "1") });
            var ctx = new EntryRenderContext { Blink = false };

            entry.Evaluate(ctx);

            Assert.AreEqual(true, ctx.Blink);
        }

        [TestMethod]
        public void DisablesBlinkMode()
        {
            var entry = new Blink(null, new[] { new Argument(null, "0") });
            var ctx = new EntryRenderContext { Blink = true };

            entry.Evaluate(ctx);

            Assert.AreEqual(false, ctx.Blink);
        }
    }
}
