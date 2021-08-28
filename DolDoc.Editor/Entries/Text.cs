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
            else
                return $"$TX,\"{Tag}\"$";
        }

        public override void CharKeyPress(ViewerState state, char key, int relativeOffset)
        {
            if (!char.IsControl(key))
            {
                Arguments[0].Value = Arguments[0].Value.Insert(relativeOffset, new string(key, 1));
                //state.CursorPosition++;
                state.Cursor.Right();
            }
            else if (key == '\r')
            {
                Arguments[0].Value = Arguments[0].Value.Insert(relativeOffset, new string('\n', 1));
            }
        }

        public override void KeyPress(ViewerState state, ConsoleKey key, int relativeOffset)
        {
            switch (key)
            {
                case ConsoleKey.Delete:
                    Arguments[0].Value = Arguments[0].Value.Remove(relativeOffset, 1);
                    break;

                case ConsoleKey.Backspace:
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
