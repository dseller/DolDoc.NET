using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Entries
{
    [Entry("PT")]
    public class Prompt : DocumentEntry
    {
        private bool jumped;
        private StringBuilder builder;

        public Prompt(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
            jumped = false;
            builder = new StringBuilder(Tag ?? string.Empty);
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var toRender = builder.ToString() + " ";

            var len = WriteString(ctx, toRender);
            if (!jumped)
            {
                ctx.State.Cursor.SetPosition(ctx.RenderPosition);
                jumped = true;
            }
            return new CommandResult(true, len);
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            if (key == Key.ENTER)
            {
                state.Document.PromptEntered(builder.ToString());
                state.Cursor.Down();
                return;
            }
            else if (key == Key.BACKSPACE)
            {
                if (builder.Length <= 0)
                    return;

                builder.Remove(relativeOffset - 1, 1);
                state.Cursor.SetPosition(state.Cursor.DocumentPosition - 1);
            }

            if (!character.HasValue)
                return;
            state.Cursor.SetPosition(state.Cursor.DocumentPosition + 1);

            builder.Insert(relativeOffset, character.Value);
            // Tag = builder.ToString();
            SetArgument(null, builder.ToString());
        }

        public override string ToString() => AsString("PT");
    }
}
