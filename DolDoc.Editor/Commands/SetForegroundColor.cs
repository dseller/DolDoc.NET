﻿using DolDoc.Editor.Core;
using System;

namespace DolDoc.Editor.Commands
{
    public class SetForegroundColor : IDolDocCommand
    {
        public CommandResult Execute(CommandContext ctx)
        {
            if (ctx.Arguments.Count == 0)
                ctx.ForegroundColor = ctx.DefaultForegroundColor;
            else
                ctx.ForegroundColor = (EgaColor)Enum.Parse(typeof(EgaColor), ctx.Arguments[0].Value, true);

            return new CommandResult(true);
        }
    }
}
