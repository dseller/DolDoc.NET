using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class Invert : IDolDocCommand
    {
        public CommandResult Execute(CommandContext ctx)
        {
            ctx.Inverted = ctx.Arguments[0].Value == "1";

            return new CommandResult(true);
        }
    }
}
