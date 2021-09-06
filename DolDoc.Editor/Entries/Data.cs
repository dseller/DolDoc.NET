using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using DolDoc.Editor.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public class Data : DocumentEntry, IFormEntry
    {
        private StringBuilder stringBuilder;

        public Data(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
            stringBuilder = new StringBuilder();
        }

        private int InputLineLength => int.Parse(GetArgument("ILL", "16"));

        public object Value => stringBuilder.ToString();

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var inverted = ctx.Inverted;
            if (Selected)
                ctx.Inverted = true;

            //var inputLine = new string(' ', InputLineLength);
            var charsWritten = WriteString(ctx, $"{Aux}: ");
            ctx.RenderPosition += charsWritten;

            var underLine = ctx.Underline;
            ctx.Underline = true;


            // charsWritten += WriteString(ctx, inputLine);
            var value = stringBuilder.ToString();
            if (value.Length > InputLineLength)
                value = value.Substring(Math.Max(0, value.Length - InputLineLength), InputLineLength);
            var inputText = value.PadRight(InputLineLength);
            charsWritten += WriteString(ctx, inputText);
            
            
            ctx.Underline = underLine;
            ctx.Inverted = inverted;

            return new CommandResult(true, charsWritten);
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            if (key == Key.BACKSPACE && stringBuilder.Length > 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                return;
            }

            if (!character.HasValue)
                return;

            stringBuilder.Append(character.Value);
        }

        public override string ToString() => AsString("DA"); //$"$DA,A=\"{Aux}\"$";
    }
}
