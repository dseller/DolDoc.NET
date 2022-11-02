using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DolDoc.Tests.Core
{
    [TestClass]
    public class CharacterPageDirectoryTests
    {
        [TestMethod]
        public void ItCreatesInitialPage()
        {
            var pd = new CharacterPageDirectory(100, 100);

            Assert.AreEqual(1, pd.PageCount);
        }

        [TestMethod]
        public void ItRetrievesInitialPage()
        {
            var pd = new CharacterPageDirectory(100, 100);

            var page = pd.GetOrCreatePage(0, 0);

            Assert.IsNotNull(page);
            Assert.AreEqual(1, page.PageNumber);
            Assert.AreEqual(100, page.Columns);
            Assert.AreEqual(100, page.Rows);
        }

        [TestMethod]
        public void ItCreatesNewPages()
        {
            var pd = new CharacterPageDirectory(10, 10);

            var page1 = pd.GetOrCreatePage(0, 15);
            Assert.IsNotNull(page1);
            Assert.AreEqual(2, page1.PageNumber);

            var page2 = pd.GetOrCreatePage(0, 51);
            Assert.IsNotNull(page2);
            Assert.AreEqual(6, page2.PageNumber);

            Assert.AreEqual(6, pd.PageCount);
        }

        [TestMethod]
        public void ItNormalizesLocalPageCoordinates()
        {
            var pd = new CharacterPageDirectory(10, 1);
            pd[5, 6].Write(new Text(null, null), 0, 0xFF, null, CharacterFlags.None);

            Assert.AreEqual(7, pd.PageCount);
            Assert.AreEqual(0, pd.Get(6)[4, 0].Char);
            Assert.AreEqual(0xFF, pd.Get(6)[5, 0].Char);
        }

        [TestMethod]
        public void ItNormalizesLocalPagePosition()
        {
            var pd = new CharacterPageDirectory(80, 60);
            pd[4801].Write(new Text(null, null), 0, 0xFF, null, CharacterFlags.None);

            Assert.AreEqual(2, pd.PageCount);
            Assert.AreEqual(0, pd.Get(1)[0, 59].Char);
            Assert.AreEqual(0xFF, pd.Get(1)[1].Char);
        }

        [TestMethod]
        public void PageCoordinatesAndPositionsMatch()
        {
            var pd = new CharacterPageDirectory(80, 60);
            pd[0, 60].Write(new Text(null, null), 0, 0xFF, null, CharacterFlags.None);

            Assert.AreEqual(2, pd.GetOrCreatePage(60 * 80).PageNumber);
            Assert.AreEqual(0xFF, pd.Get(1)[0].Char);
            Assert.AreEqual(0xFF, pd[60 * 80].Char);
        }

        [TestMethod]
        public void Test80x60Pages()
        {
            var pd = new CharacterPageDirectory(80, 60);

            for (int i = 0; i < 80 * 60; i++)
                pd[i].Write(new Text(null, null), 0, (byte)'A', null, CharacterFlags.None);
            for (int i = 80*60; i < (80*60)*2; i++)
                pd[i].Write(new Text(null, null), 0, (byte)'B', null, CharacterFlags.None);

            for (int i = 0; i < 80 * 60; i++)
                Assert.AreEqual((byte)'A', pd[i].Char);
        }
    }
}
