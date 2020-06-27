using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class WordWrap : IDolDocCommand
    {
        public CommandResult Execute(CommandContext ctx)
        {
            ctx.WordWrap = ctx.Arguments[0].Value == "1";

            return new CommandResult(true);
        }
    }
}
