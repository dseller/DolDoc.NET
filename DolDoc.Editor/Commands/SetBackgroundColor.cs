using DolDoc.Editor.Core;
using System;

namespace DolDoc.Editor.Commands
{
    public class SetBackgroundColor : IDolDocCommand
    {
        public CommandResult Execute(DocumentEntry entry, CommandContext ctx)
        {
            if (entry.Arguments.Count == 0)
                ctx.BackgroundColor = ctx.DefaultBackgroundColor;
            else
                ctx.BackgroundColor = (EgaColor)Enum.Parse(typeof(EgaColor), entry.Arguments[0].Value, true);

            return new CommandResult(true);
        }
    }
}
