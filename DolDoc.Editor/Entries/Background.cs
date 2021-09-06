using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
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
                ctx.Options.BackgroundColor = (EgaColor)Enum.Parse(typeof(EgaColor), Tag, true);

            return new CommandResult(true);
        }

        public override string ToString() => AsString("BG");
    }
}
