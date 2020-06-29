using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class Indent : IDolDocCommand
    {
        public CommandResult Execute(DocumentEntry entry, CommandContext ctx)
        {
            var value = int.Parse(entry.Arguments[0].Value);

            ctx.Indentation += value;
            if (ctx.Indentation < 0)
                ctx.Indentation = 0;

            return new CommandResult(true);
        }
    }
}
