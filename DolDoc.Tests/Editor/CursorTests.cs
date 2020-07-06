using DolDoc.Editor;
using DolDoc.Editor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        public void MoveLeft_DoesNotMovePastTheBeginning()
        {
            var doc = new Document("$TX,\"Hello World!\"$");
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            doc.Refresh();

            cursor.Left();

            Assert.AreEqual(0, cursor.DocumentPosition);
        }

        [TestMethod]
        public void MoveLeft_MovesWithinSameEntry()
        {
            var doc = new Document("$TX,\"Hello World!\"$");
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            cursor.DocumentPosition = 5;
            doc.Refresh();

            cursor.Left();

            Assert.AreEqual(doc.Entries.First.Value, cursor.SelectedEntry);
            Assert.AreEqual(4, cursor.DocumentPosition);
        }

        [TestMethod]
        public void MoveLeft_ToPreviousEntry()
        {
            /*
             * ABC--------------------------------Centered-------------------------------------
             */

            var doc = new Document("$TX,\"ABC\"$$TX+CX,\"Centered\"$");
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            cursor.DocumentPosition = 36;
            doc.Refresh();

            Assert.AreEqual(doc.Entries.First.Next.Value, cursor.SelectedEntry);

            cursor.Left();

            Assert.AreEqual(2, cursor.DocumentPosition);
            Assert.AreEqual(doc.Entries.First.Value, cursor.SelectedEntry);
        }

        [TestMethod]
        public void MoveUp_DoesNotMovePastTheBeginning()
        {
            var doc = new Document("$TX,\"Hello World!\"$");
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            cursor.DocumentPosition = 2;
            doc.Refresh();

            cursor.Up();

            Assert.AreEqual(2, cursor.DocumentPosition);
        }

        [TestMethod]
        public void MoveUp_ToPreviousEntry()
        {
            var doc = new Document("$TX,\"ABC\"$\nDEF");
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            cursor.DocumentPosition = state.Columns + 2;
            doc.Refresh();

            Assert.AreEqual(doc.Entries.First.Next.Value, cursor.SelectedEntry);

            cursor.Up();

            Assert.AreEqual(2, cursor.DocumentPosition);
            Assert.AreEqual(doc.Entries.First.Value, cursor.SelectedEntry);
        }

        [TestMethod]
        public void MoveUp_WithinSameEntry()
        {
            var doc = new Document(new string('A', 80 * 2));
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            cursor.DocumentPosition = state.Columns + 1;
            doc.Refresh();

            Assert.AreEqual(doc.Entries.First.Value, cursor.SelectedEntry);

            cursor.Up();

            Assert.AreEqual(1, cursor.DocumentPosition);
            Assert.AreEqual(doc.Entries.First.Value, cursor.SelectedEntry);
        }

        [TestMethod]
        public void MoveDown_DoesNotMovePastEndOfContent()
        {
            var doc = new Document("$TX,\"Hello World!\"$");
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            doc.Refresh();

            cursor.Down();

            Assert.AreEqual(0, cursor.DocumentPosition);
        }

        [TestMethod]
        public void MoveDown_WithinSameEntry()
        {
            var doc = new Document(new string('A', 80 * 2));
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            doc.Refresh();

            Assert.AreEqual(doc.Entries.First.Value, cursor.SelectedEntry);

            cursor.Down();

            Assert.AreEqual(state.Columns, cursor.DocumentPosition);
            Assert.AreEqual(doc.Entries.First.Value, cursor.SelectedEntry);
        }

        [TestMethod]
        public void MoveDown_ToNextEntry()
        {
            var doc = new Document("$TX,\"ABC\"$\nDEF");
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            doc.Refresh();

            Assert.AreEqual(doc.Entries.First.Value, cursor.SelectedEntry);

            cursor.Down();

            Assert.AreEqual(state.Columns, cursor.DocumentPosition);
            Assert.AreEqual(doc.Entries.First.Next.Value, cursor.SelectedEntry);
        }

        [TestMethod]
        public void MoveDown_UpdatesViewLine()
        {
            var doc = new Document(new string('A', 80 * 60 * 2));
            var state = new ViewerState(null, doc, 640, 480);
            var cursor = new Cursor(state);
            doc.Refresh();

            Assert.AreEqual(doc.Entries.First.Value, cursor.SelectedEntry);

            for (int i = 0; i < 62; i++)
                cursor.Down();

            Assert.AreEqual(2, cursor.ViewLine);
        }
    }
}
