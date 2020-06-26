using DolDoc.Editor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DolDoc.Tests.Editor
{
    [TestClass]
    public class PieceTableTests
    {
        [TestMethod]
        public void ItLoadsContent()
        {
            var tbl = new PieceTable("original");

            Assert.AreEqual("original", tbl.Original);
            Assert.AreEqual(1, tbl.Pieces.Count);
            Assert.AreEqual(0, tbl.Pieces[0].Start);
            Assert.AreEqual("original".Length, tbl.Pieces[0].Length);
            Assert.AreEqual(false, tbl.Pieces[0].AddBuffer);
        }

        [TestMethod]
        public void ItReturnsTheString()
        {
            var tbl = new PieceTable("original");

            Assert.AreEqual("original", tbl.ToString());
        }

        [TestMethod]
        public void ItInsertsTextAtTheBeginning()
        {
            var tbl = new PieceTable("world");
            tbl.Insert("Hello ", 0);

            Assert.AreEqual("world", tbl.Original);
            Assert.AreEqual("Hello ", tbl.Add);
            Assert.AreEqual(2, tbl.Pieces.Count);

            Assert.AreEqual("Hello world", tbl.ToString());
        }

        [TestMethod]
        public void ItInsertsTextAtTheEnd()
        {
            var tbl = new PieceTable("Hello");
            tbl.Insert(" world", tbl.ToString().Length);

            Assert.AreEqual("Hello", tbl.Original);
            Assert.AreEqual(" world", tbl.Add);
            Assert.AreEqual(2, tbl.Pieces.Count);

            Assert.AreEqual("Hello world", tbl.ToString());
        }

        [TestMethod]
        public void ItInsertsTextInTheMiddle()
        {
            var tbl = new PieceTable("Lorem dolor sit amet");
            tbl.Insert(" ipsum", 5);

            Assert.AreEqual("Lorem ipsum dolor sit amet", tbl.ToString());
        }

        [TestMethod]
        public void ItInsertsMultipleTexts()
        {
            var tbl = new PieceTable();
            tbl.Insert("Hello world", 0);
            tbl.Insert(" wonderful", 5);
            tbl.Insert(", this is a test!", tbl.ToString().Length);

            Assert.AreEqual("Hello wonderful world, this is a test!", tbl.ToString());
        }
        
        [TestMethod]
        public void ItRemovesText()
        {
            var tbl = new PieceTable("Hello asdasd world");
            tbl.Remove(6, 7);

            Assert.AreEqual("Hello world", tbl.ToString());
        }

        [TestMethod]
        public void ItRemovesTextAtTheBeginning()
        {
            var tbl = new PieceTable("asdasd Hello world");
            tbl.Remove(0, 7);

            Assert.AreEqual("Hello world", tbl.ToString());
        }

        [TestMethod]
        public void ItRemovesTextAtTheEnd()
        {
            var tbl = new PieceTable("Hello worldasdasd");
            tbl.Remove(11, 6);

            Assert.AreEqual("Hello world", tbl.ToString());
        }

        [TestMethod]
        public void ItInsertsAndRemovesMultipleTexts()
        {
            var tbl = new PieceTable();
            tbl.Insert("Hello world", 0);
            tbl.Insert(" asdasd wonderful", 5);
            tbl.Insert(", this is a ajslkdhk test!", tbl.ToString().Length);
            tbl.Remove(5, 7);
            tbl.Remove(33, "ajslkdhk ".Length);

            Assert.AreEqual("Hello wonderful world, this is a test!", tbl.ToString());
        }
    }
}
