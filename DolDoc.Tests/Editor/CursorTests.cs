using DolDoc.Editor;
using DolDoc.Editor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Tests.Editor
{
    [TestClass]
    public class CursorTests
    {
        [TestMethod]
        public void MoveRight_WithinSameEntry()
        {
            var doc = new Document("$TX,\"Hello World!\"$");
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            doc.Refresh();

            cursor.Right();

            Assert.AreEqual(1, cursor.DocumentPosition);
        }

        [TestMethod]
        public void MoveRight_ToNextEntry()
        {
            /*
             * ABC--------------------------------Centered-------------------------------------
             */

            var doc = new Document("$TX,\"ABC\"$$TX+CX,\"Centered\"$");
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            doc.Refresh();

            cursor.Right();
            cursor.Right();
            cursor.Right();

            Assert.AreEqual(36, cursor.DocumentPosition);
        }

        [TestMethod]
        public void MoveRight_DoesNotMovePastTheEnd()
        {
            var doc = new Document("$TX,\"ABC\"$");
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            doc.Refresh();

            for (int i = 0; i < 10; i++)
                cursor.Right();

            Assert.AreEqual(2, cursor.DocumentPosition);
        }
    }
}
