using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    [Entry("BG")]
    public class Background : DocumentEntry
    {
        public Background(IList<Flag> flags, IList<Argument> args) 
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            if (Arguments.Count == 0)
                ctx.Options.BackgroundColor = ctx.Options.DefaultBackgroundColor;
            else
            {
                if (Enum.TryParse<EgaColor>(Tag, true, out var color))
                    ctx.Options.BackgroundColor = color;
                else if (byte.TryParse(Tag, out var colorInt))
                    ctx.Options.BackgroundColor = (EgaColor)colorInt;
            }

            return new CommandResult(true);
        }

        public override string ToString() => AsString("BG");
    }
}
