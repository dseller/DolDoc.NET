//using DolDoc.Editor.Commands;
//using DolDoc.Editor.Core;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace DolDoc.Editor.Entries
//{
//    public class Prompt : DocumentEntry
//    {
//        private string prompt;
//        private char? ch;

//        public Prompt(IList<Flag> flags, IList<Argument> args) : base(flags, args)
//        {
//            prompt = string.Empty;
//            ch = null;
//        }

//        public override CommandResult Evaluate(EntryRenderContext ctx)
//        {
//            while (!ch.HasValue && ch.Value != '\n')
//            {

//            }

//            while (!charEntered)
//                Thread.Sleep(250);
//            charEntered = false;

//            WriteString(ctx, prompt);
//            return new CommandResult(true, prompt.Length);
//        }

//        public override void CharKeyPress(ViewerState state, char key, int relativeOffset)
//        {
//            prompt += key;
//            charEntered = true;
//        }

//        public override string ToString() => "";
//    }
//}
