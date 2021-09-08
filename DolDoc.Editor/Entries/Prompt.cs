using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public class Prompt : DocumentEntry
    {
        private bool jumped;
        private StringBuilder builder;

        public Prompt(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
            jumped = false;
            builder = new StringBuilder();
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var toRender = builder.ToString() + " ";

            var len = WriteString(ctx, toRender);
            if (!jumped)
            {
                ctx.State.Cursor.DocumentPosition = ctx.RenderPosition;
                jumped = true;
            }
            return new CommandResult(true, len);
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            if (key == Key.ENTER)
            {
                state.Document.PromptEntered(builder.ToString());
                return;
            }

            if (!character.HasValue)
                return;
            state.Cursor.DocumentPosition++;
            builder.Append(character.Value);
        }

        public override string ToString() => AsString("PT");
    }
}
