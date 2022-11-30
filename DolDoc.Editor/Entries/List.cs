using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using DolDoc.Editor.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DolDoc.Editor.Entries
{
    [Entry("LS")]
    public class List : DocumentEntry
    {
        private int selectedIndex;
        private Dictionary<string, object> values;

        public List(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
            selectedIndex = 0;

            values = new Dictionary<string, object>();
            var type = Enum.Parse<ListFieldSource>(GetArgument("TYPE"));

            switch (type)
            {
                case ListFieldSource.Enum:
                    foreach (var t in Enum.GetValues(Type.GetType(GetArgument("SRC"))))
                        values.Add(t.ToString(), t);
                    break;
            }

            Debug.WriteLine($"List widget: {values}");
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var options = ctx.NewOptions();
            if (Selected)
                options.Inverted = true;

            var key = values.Keys.ToArray()[selectedIndex];
            var length = values.Values.Max(value => value.ToString().Length);
            var writtenChars = WriteString(ctx, $"{Aux}: [{values[key].ToString().PadLeft(length)}]");

            ctx.PopOptions();
            return new CommandResult(true, writtenChars);
        }

        public override void KeyPress(ViewerState state, Key key, char? character, int relativeOffset)
        {
            if (key == Key.SPACE || key == Key.ENTER)
                SelectNextValue(state);
        }

        public override void Click(ViewerState state) => SelectNextValue(state);

        public override bool Clickable => true;

        public override string ToString() => AsString("LS");

        private void SelectNextValue(ViewerState state)
        {
            selectedIndex++;
            if (selectedIndex >= values.Count)
                selectedIndex = 0;

            var k = values.Keys.ToArray()[selectedIndex];
            state.Document.FieldChanged(GetArgument("PROP"), values[k]);
        }
    }
}
