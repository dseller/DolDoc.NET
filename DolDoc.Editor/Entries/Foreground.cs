using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    [Entry("FG")]
    public class Foreground : DocumentEntry
    {
        public Foreground(IList<Flag> flags, IList<Argument> args)
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            if (Arguments.Count == 0)
                ctx.Options.ForegroundColor = ctx.Options.DefaultForegroundColor;
            else
            {
                if (Enum.TryParse<EgaColor>(Tag, true, out var color))
                    ctx.Options.ForegroundColor = color;
                else if (byte.TryParse(Tag, out var colorInt))
                    ctx.Options.ForegroundColor = (EgaColor)colorInt;
            }

            return new CommandResult(true);
        }

        public override string ToString() => AsString("FG");
    }
}
