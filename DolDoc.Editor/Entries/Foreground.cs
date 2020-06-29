using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Foreground : DocumentEntry
    {
        public Foreground(int textOffset, IList<Flag> flags, IList<Argument> args)
            : base(textOffset, flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            if (Arguments.Count == 0)
                ctx.ForegroundColor = ctx.DefaultForegroundColor;
            else
                ctx.ForegroundColor = (EgaColor)Enum.Parse(typeof(EgaColor), Tag, true);

            return new CommandResult(true);
        }

        public override string ToString() => $"$FG,{Tag}$";
    }
}
