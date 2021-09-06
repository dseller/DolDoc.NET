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
            var ctx = new EntryRenderContext(null, null, new RenderOptions { Blink = false });

            entry.Evaluate(ctx);

            Assert.AreEqual(true, ctx.Options.Blink);
        }

        [TestMethod]
        public void DisablesBlinkMode()
        {
            var entry = new Blink(null, new[] { new Argument(null, "0") });
            var ctx = new EntryRenderContext(null, null, new RenderOptions { Blink = true });

            entry.Evaluate(ctx);

            Assert.AreEqual(false, ctx.Options.Blink);
        }
    }
}
