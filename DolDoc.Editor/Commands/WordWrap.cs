using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class WordWrap : IDolDocCommand
    {
        public CommandResult Execute(DocumentEntry entry, CommandContext ctx)
        {
            ctx.WordWrap = entry.Arguments[0].Value == "1";

            return new CommandResult(true);
        }
    }
}
