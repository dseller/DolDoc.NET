using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    [Entry("DA")]
    public class Data : DocumentEntry
    {
        public Data(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
        }

        private int InputLineLength => int.Parse(GetArgument("ILL", "16"));

        private string Property => GetArgument("PROP") ?? Aux;

        public override bool IsInput => true;

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var options = ctx.NewOptions();
            if (Selected)
                options.Inverted = true;

            var charsWritten = WriteString(ctx, $"{Aux}: ");
            ctx.RenderPosition += charsWritten;
            
            options.Underline = true;
            var value = ctx.Document.GetData(Property)?.ToString() ?? string.Empty;
            if (value.Length > InputLineLength)
                value = value.Substring(Math.Max(0, value.Length - InputLineLength), InputLineLength);
            var inputText = value.PadRight(InputLineLength);
            charsWritten += WriteString(ctx, inputText);
            
            ctx.PopOptions();
            return new CommandResult(true, charsWritten);
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            var value = state.Document.GetData(Property)?.ToString() ?? string.Empty;
            if (key == Key.BACKSPACE && value.Length > 0)
            {
                value = value.Remove(value.Length - 1, 1);
                state.Document.FieldChanged(Property, value);
                return;
            }

            if (!character.HasValue)
                return;

            value += character.Value;
            state.Document.FieldChanged(Property, value);
        }

        public override string ToString() => AsString("DA");
    }
}
