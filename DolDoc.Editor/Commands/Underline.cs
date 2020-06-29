using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class Underline : IDolDocCommand
    {
        public CommandResult Execute(DocumentEntry entry, CommandContext ctx)
        {
            ctx.Underline = entry.Arguments[0].Value == "1";

            return new CommandResult(true);
        }
    }
}
