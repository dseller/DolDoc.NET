using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class Blink : IDolDocCommand
    {
        public CommandResult Execute(CommandContext ctx)
        {
            ctx.Blink = ctx.Arguments[0].Value == "1";

            return new CommandResult(true);
        }
    }
}
