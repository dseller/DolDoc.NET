using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class Blink : IDolDocCommand
    {
        public CommandResult Execute(DocumentEntry entry, CommandContext ctx)
        {
            ctx.Blink = entry.Arguments[0].Value == "1";

            return new CommandResult(true);
        }
    }
}
