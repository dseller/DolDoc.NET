using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public class CheckBox : DocumentEntry
    {
        public CheckBox(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        private string Property => GetArgument("PROP");

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var @checked = ctx.Document.GetData(Property) as bool?;// ?? false;
            if (!@checked.HasValue && HasArgument("RE"))
            {
                if (int.TryParse(GetArgument("RE"), out var checkedInt))
                    @checked = checkedInt == 1;
                else
                    @checked = false;
            }

            var options = ctx.NewOptions();
            if (Selected)
                options.Inverted = true;

            var builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Tag))
                builder.Append($"{Tag}: ");
            builder.Append($"[{(@checked.Value ? "X" : " ")}]");
            var writtenChars = WriteString(ctx, builder.ToString());

            ctx.PopOptions();
            return new CommandResult(true, writtenChars);
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            if (key == Key.SPACE || key == Key.ENTER)
                Toggle(state);
            else
                base.KeyPress(state, key, character, relativeOffset);
        }

        public override void Click(ViewerState state) => Toggle(state);

        public override bool Clickable => true;

        public override string ToString() => AsString("CB");

        private void Toggle(ViewerState state)
        {
            // Invoke macro call if an LM argument is provided.
            if (HasArgument("LM"))
                state.Document.Macro(this);

            var @checked = state.Document.GetData(Property) as bool? ?? false;
            state.Document.FieldChanged(GetArgument("PROP"), !@checked);
        }
    }
}
