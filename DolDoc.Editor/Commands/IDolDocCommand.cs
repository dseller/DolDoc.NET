﻿using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Commands
{
    public interface IDolDocCommand
    {
        CommandResult Execute(CommandContext ctx);
    }
}
