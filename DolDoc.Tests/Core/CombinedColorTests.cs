using DolDoc.Editor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Tests.Core
{
    [TestClass]
    public class CombinedColorTests
    {
        [TestMethod]
        public void CorrectlySetsTheValue()
        {
            var color = new CombinedColor(EgaColor.LtBlue, EgaColor.Cyan);

            Assert.AreEqual(EgaColor.Cyan, color.Foreground);
            Assert.AreEqual(EgaColor.LtBlue, color.Background);
        }
    }
}
