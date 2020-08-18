using DolDoc.Editor;
using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DolDoc.Tests.Commands
{
    [TestClass]
    public class WriteStringTests
    {
        private const string LoremIpsum = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla auctor convallis ipsum ut malesuada. Pellentesque sed erat nisl. In lacinia nibh ac vestibulum aliquet. Donec malesuada ut ligula a semper. Etiam dapibus risus libero, egestas viverra erat facilisis sed. Sed ex nulla, elementum eu rutrum eu, vestibulum ac massa. Mauris eget erat turpis. Curabitur tempor sagittis velit ut tincidunt. Sed eleifend dictum euismod. Mauris sed quam sit amet turpis pellentesque viverra.";

        private Document _document;
        private EntryRenderContext _commandContext;

        [TestInitialize]
        public void Setup()
        {
            _document = new Document();
            _commandContext = new EntryRenderContext
            {
                State = new ViewerState(null, _document, 640, 480)
            };
        }

        [TestMethod]
        public void WritesSimpleString()
        {
            const string str = "Hello World!";

            _document.Load(str);

            var ws = _document.Entries.First;
            var result = ws.Value.Evaluate(_commandContext);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(str.Length, result.WrittenCharacters);

            for (var i = 0; i < str.Length; i++)
            {
                Assert.AreEqual(_document.Entries.First(), _commandContext.State.Pages[i].Entry);
                Assert.AreEqual((byte)str[i], _commandContext.State.Pages[i].Char);
            }
        }

        [TestMethod]
        public void WritesWordWrapString()
        {
            /*
             * Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla auctor convallis-     0
             * ipsum ut malesuada. Pellentesque sed erat nisl. In lacinia nibh ac vestibulum-__     2
             * aliquet. Donec malesuada ut ligula a semper. Etiam dapibus risus libero,-_______     7
             * egestas viverra erat facilisis sed. Sed ex nulla, elementum eu rutrum eu,-______     6
             * vestibulum ac massa. Mauris eget erat turpis. Curabitur tempor sagittis velit-__     2
             * ut tincidunt. Sed eleifend dictum euismod. Mauris sed quam sit amet turpis-_____     5
             * pellentesque viverra. 
             */

            _commandContext.WordWrap = true;

            _document.Load(LoremIpsum);
            var ws = _document.Entries.First;
            var result = ws.Value.Evaluate(_commandContext);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(LoremIpsum.Length + 22, result.WrittenCharacters);
        }
    }
}
