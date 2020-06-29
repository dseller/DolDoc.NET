using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DolDoc.Interpreter.Commands
{
    public static class CommandHelper
    {
        private static readonly IDictionary<DocumentCommand, IDolDocCommand> commands =
            new Dictionary<DocumentCommand, IDolDocCommand>()
            {
                { DocumentCommand.Background, new SetBackgroundColor() },
                { DocumentCommand.Blink, new Blink() },
                { DocumentCommand.Clear, new Clear() },
                { DocumentCommand.MoveCursor, new CursorMove() },
                { DocumentCommand.Foreground, new SetForegroundColor() },
                { DocumentCommand.Invert, new Invert() },
                { DocumentCommand.Link, new Link() },
                { DocumentCommand.Text, new WriteString() },
                { DocumentCommand.Underline, new Underline() }
            };

        public static CommandResult Execute(DocumentEntry cmd, CommandContext ctx)
        {
            if (!commands.TryGetValue(cmd.Command, out var command))
                return new CommandResult(false);

            return command.Execute(cmd, ctx);
        }
    }
}
