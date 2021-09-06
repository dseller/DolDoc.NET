using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Text : DocumentEntry
    {
        public Text(IList<Flag> flags, IList<Argument> args)
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx) => new CommandResult(true, WriteString(ctx, Tag));

        public override string ToString()
        {
            if (Arguments.Count == 1)
                return Tag;
            return AsString("TX");
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            if (!char.IsControl((char)key))
            {
                Arguments[0].Value = Arguments[0].Value.Insert(relativeOffset, new string((char)key, 1));
                //state.CursorPosition++;
                state.Cursor.Right();
            }

            switch (key)
            {
                case Key.ENTER:
                    Arguments[0].Value = Arguments[0].Value.Insert(relativeOffset, new string('\n', 1));
                    break;

                case Key.DEL:
                    Arguments[0].Value = Arguments[0].Value.Remove(relativeOffset, 1);
                    break;

                case Key.BACKSPACE:
                    if (relativeOffset > 0)
                    {
                        Arguments[0].Value = Arguments[0].Value.Remove(relativeOffset - 1, 1);
                        //state.CursorPosition--;
                        state.Cursor.Left();
                    }
                    else if (state.Pages[state.CursorPosition - 1].HasEntry)
                    {
                        state.Document.Entries.Remove(state.Pages[state.CursorPosition - 1].Entry);
                        // TODO: update cursor minus length of entry
                        // state.CursorPosition -=  
                    }
                    
                    break;
            }
        }
    }
}
