using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class Invert : IDolDocCommand
    {
        public CommandResult Execute(DocumentEntry entry, CommandContext ctx)
        {
            ctx.Inverted = entry.Arguments[0].Value == "1";

            return new CommandResult(true);
        }
    }
}
