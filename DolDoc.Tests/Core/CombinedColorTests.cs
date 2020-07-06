using DolDoc.Editor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DolDoc.Tests.Core
{
    [TestClass]
    public class CombinedColorTests
    {
        [TestMethod]
        public void CorrectlySetsTheInitialValue()
        {
            var color = new CombinedColor(EgaColor.LtBlue, EgaColor.Cyan);

            Assert.AreEqual(EgaColor.Cyan, color.Foreground);
            Assert.AreEqual(EgaColor.LtBlue, color.Background);
        }

        [TestMethod]
        public void CorrectlySetsTheBackgroundColor()
        {
            var color = new CombinedColor(EgaColor.LtBlue, EgaColor.Cyan);
            color.Background = EgaColor.Yellow;

            Assert.AreEqual(EgaColor.Cyan, color.Foreground);
            Assert.AreEqual(EgaColor.Yellow, color.Background);
        }

        [TestMethod]
        public void CorrectlySetsTheForegroundColor()
        {
            var color = new CombinedColor(EgaColor.LtBlue, EgaColor.Cyan);
            color.Foreground = EgaColor.Yellow;

            Assert.AreEqual(EgaColor.Yellow, color.Foreground);
            Assert.AreEqual(EgaColor.LtBlue, color.Background);
        }
    }
}
