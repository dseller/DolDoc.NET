//using DolDoc.Interpreter.Domain;
//using DolDoc.Interpreter.Parser;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace DolDoc.Tests.Interpreter
//{
//    [TestClass]
//    public class LegacyParserTests
//    {
//        private LegacyParser _parser;

//        [TestInitialize]
//        public void Initialize()
//        {
//            _parser = new LegacyParser();
//        }

//        [TestMethod]
//        public void EmitsStrings()
//        {
//            string str = null;
//            _parser.OnWriteString += value => str = value;

//            const string text = "$FG,BLUE$Hello World!";
//            using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(text)))
//            {
//                _parser.Parse(ms);
//            }

//            Assert.AreEqual("Hello World!", str);
//        }

//        [TestMethod]
//        public void EmitsEscapedDollars()
//        {
//            char ch = default;
//            _parser.OnWriteCharacter += value => ch = value;

//            const string text = "$FG,BLUE$$$";
//            using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(text)))
//            {
//                _parser.Parse(ms);
//            }

//            Assert.AreEqual('$', ch);
//        }

//        [TestMethod]
//        public void EmitsCommands()
//        {
//            List<Command> cmds = new List<Command>();
//            _parser.OnCommand += value => cmds.Add(value);

//            const string text = "$FG,BLUE$Hello World!$BG,RED$";
//            using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(text)))
//            {
//                _parser.Parse(ms);
//            }

//            Assert.AreEqual(2, cmds.Count);
//            Assert.AreEqual("FG", cmds[0].Mnemonic);
//            Assert.AreEqual("BLUE", cmds[0].Arguments.ElementAt(0).Value);
//            Assert.AreEqual("BG", cmds[1].Mnemonic);
//            Assert.AreEqual("RED", cmds[1].Arguments.ElementAt(0).Value);
//        }

//        [TestMethod]
//        public void ParsesQuotedArguments()
//        {
//            Command cmd = null;
//            _parser.OnCommand += value => cmd = value;

//            const string text = "$LK,\"Test Quoted String\",\"Another\"$";
//            using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(text)))
//            {
//                _parser.Parse(ms);
//            }

//            Assert.AreEqual("LK", cmd.Mnemonic);
//            Assert.AreEqual("Test Quoted String", cmd.Arguments.ElementAt(0).Value);
//            Assert.AreEqual("Another", cmd.Arguments.ElementAt(1).Value);
//        }

//        [TestMethod]
//        public void ParsesPlusFlags()
//        {
//            Command cmd = null;
//            _parser.OnCommand += value => cmd = value;

//            const string text = "$TX+CX+DX$";
//            using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(text)))
//            {
//                _parser.Parse(ms);
//            }

//            Assert.AreEqual("TX", cmd.Mnemonic);
//            Assert.AreEqual(true, cmd.Flags.ElementAt(0).Status);
//            Assert.AreEqual("CX", cmd.Flags.ElementAt(0).Value);
//            Assert.AreEqual(true, cmd.Flags.ElementAt(1).Status);
//            Assert.AreEqual("DX", cmd.Flags.ElementAt(1).Value);
//        }

//        [TestMethod]
//        public void ParsesMinusFlags()
//        {
//            Command cmd = null;
//            _parser.OnCommand += value => cmd = value;

//            const string text = "$TX-CX-DX$";
//            using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(text)))
//            {
//                _parser.Parse(ms);
//            }

//            Assert.AreEqual("TX", cmd.Mnemonic);
//            Assert.AreEqual(false, cmd.Flags.ElementAt(0).Status);
//            Assert.AreEqual("CX", cmd.Flags.ElementAt(0).Value);
//            Assert.AreEqual(false, cmd.Flags.ElementAt(1).Status);
//            Assert.AreEqual("DX", cmd.Flags.ElementAt(1).Value);
//        }
//    }
//}
