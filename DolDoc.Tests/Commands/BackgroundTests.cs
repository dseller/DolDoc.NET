using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DolDoc.Tests.Commands
{
    [TestClass]
    public class BackgroundTests
    {
        [DataTestMethod]
        [DataRow("RED", EgaColor.Red)]
        [DataRow("BLUE", EgaColor.Blue)]
        public void SetsBackgroundColor(string argument, EgaColor expected)
        {
            var entry = new Background(new List<Flag>(), new List<Argument> { new Argument(null, argument) });

            var ctx = new EntryRenderContext(null, null, new RenderOptions());
            entry.Evaluate(ctx);

            Assert.AreEqual(expected, ctx.Options.BackgroundColor);
        }

        [TestMethod]
        public void ResetsBackgroundColor()
        {
            var entry = new Background(new List<Flag>(), new List<Argument>());

            var opts = new RenderOptions();
            opts.BackgroundColor = EgaColor.Red;
            var ctx = new EntryRenderContext(null, null, opts);
            entry.Evaluate(ctx);

            Assert.AreEqual(EgaColor.White, ctx.Options.BackgroundColor);
        }
    }
}
