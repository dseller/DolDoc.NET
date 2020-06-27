using DolDoc.Core.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DolDoc.Tests.Interpreter
{
    [TestClass]
    public class LegacyParserTests
    {
        private LegacyParser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new LegacyParser();
        }

        [TestMethod]
        public void EmitsStrings()
        {
            const string text = "Hello World!";
            var result = _parser.Parse(text).ToArray();

            Assert.AreEqual("TX", result[0].Mnemonic);
            Assert.AreEqual("Hello World!", result[0].Arguments[0].Value);
        }

        [TestMethod]
        public void EmitsEscapedDollars()
        {
            const string text = "$$";
            var result = _parser.Parse(text).ToArray();

            Assert.AreEqual("$", result[0].Arguments[0].Value);
        }

        [TestMethod]
        public void EmitsCommands()
        {
            const string text = "$FG,BLUE$Hello World!$BG,RED$";
            var results = _parser.Parse(text).ToArray();

            Assert.AreEqual(3, results.Length);
            Assert.AreEqual("FG", results[0].Mnemonic);
            Assert.AreEqual("BLUE", results[0].Arguments.ElementAt(0).Value);
            Assert.AreEqual("BG", results[2].Mnemonic);
            Assert.AreEqual("RED", results[2].Arguments.ElementAt(0).Value);
        }

        [TestMethod]
        public void ParsesQuotedArguments()
        {
            const string text = "$LK,\"Test Quoted String\",\"Another\"$";
            var results = _parser.Parse(text).ToArray();

            Assert.AreEqual("LK", results[0].Mnemonic);
            Assert.AreEqual("Test Quoted String", results[0].Arguments.ElementAt(0).Value);
            Assert.AreEqual("Another", results[0].Arguments.ElementAt(1).Value);
        }

        [TestMethod]
        public void ParsesPlusFlags()
        {
            const string text = "$TX+CX+DX$";
            var results = _parser.Parse(text).ToArray();

            Assert.AreEqual("TX", results[0].Mnemonic);
            Assert.AreEqual(true, results[0].Flags.ElementAt(0).Status);
            Assert.AreEqual("CX", results[0].Flags.ElementAt(0).Value);
            Assert.AreEqual(true, results[0].Flags.ElementAt(1).Status);
            Assert.AreEqual("DX", results[0].Flags.ElementAt(1).Value);
        }

        [TestMethod]
        public void ParsesMinusFlags()
        {
            const string text = "$TX-CX-DX$";
            var results = _parser.Parse(text).ToArray();

            Assert.AreEqual("TX", results[0].Mnemonic);
            Assert.AreEqual(false, results[0].Flags.ElementAt(0).Status);
            Assert.AreEqual("CX", results[0].Flags.ElementAt(0).Value);
            Assert.AreEqual(false, results[0].Flags.ElementAt(1).Status);
            Assert.AreEqual("DX", results[0].Flags.ElementAt(1).Value);
        }

        //[Ignore("This test is ignored because the legacy parser can not handle keyed arguments")]
        [TestMethod]
        public void ParsesKeyedArguments()
        {
            var results = _parser.Parse("$TX,\"Hello\",SX=4$").ToArray();

            Assert.IsNull(results[0].Arguments.ElementAt(0).Key);
            Assert.AreEqual("Hello", results[0].Arguments.ElementAt(0).Value);
            Assert.AreEqual("SX", results[0].Arguments.ElementAt(1).Key);
        }
    }
}
