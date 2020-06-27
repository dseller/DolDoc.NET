using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class Underline : IDolDocCommand
    {
        public CommandResult Execute(CommandContext ctx)
        {
            ctx.Underline = ctx.Arguments[0].Value == "1";

            return new CommandResult(true);
        }
    }
}
