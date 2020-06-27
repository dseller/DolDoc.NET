using DolDoc.Editor;
using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;

namespace DolDoc.Interpreter.Commands
{
    public static class CommandHelper
    {
        public static CommandResult Execute(Command cmd, CommandContext ctx)
        {
            switch (cmd.Mnemonic)
            {
                case "FG": return new SetForegroundColor().Execute(ctx);
                case "LK": return new Link().Execute(ctx);
                case "TX": return new WriteString().Execute(ctx);
                case "UL": return new Underline().Execute(ctx);
                case "WW": return new WordWrap().Execute(ctx);
                default:
                    Console.WriteLine($"Unimplemented command {cmd.Mnemonic}");
                    return null;
            }
        }
    }
}
