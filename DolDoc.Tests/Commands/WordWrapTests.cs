using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DolDoc.Tests.Commands
{
    [TestClass]
    public class WordWrapTests
    {
        [TestMethod]
        public void EnablesWordWrap()
        {
            var entry = new WordWrap(null, new[] { new Argument(null, "1") });
            var ctx = new EntryRenderContext { WordWrap = false };

            entry.Evaluate(ctx);

            Assert.AreEqual(true, ctx.WordWrap);
        }

        [TestMethod]
        public void DisablesWordWrap()
        {
            var entry = new WordWrap(null, new[] { new Argument(null, "0") });
            var ctx = new EntryRenderContext { WordWrap = true };

            entry.Evaluate(ctx);

            Assert.AreEqual(false, ctx.WordWrap);
        }
    }
}
