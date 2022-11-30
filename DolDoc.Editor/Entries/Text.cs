using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    [Entry("TX")]
    public class Text : DocumentEntry
    {
        public Text(IList<Flag> flags, IList<Argument> args)
            : base(flags, args)
        {
        }

        public static Text Create(IList<Flag> flags, string value) => new Text(flags, new[] { new Argument(null, value) });

        public override CommandResult Evaluate(EntryRenderContext ctx) => new CommandResult(true, WriteString(ctx, Tag));

        public override string ToString()
        {
            if (Arguments.Count == 1 && Flags.Count == 0)
                return Tag;
            return AsString("TX");
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            if (character.HasValue)
            {
                Arguments[0].Value = Arguments[0].Value.Insert(relativeOffset, new string(character.Value, 1));
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
