using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class Indent : IDolDocCommand
    {
        public CommandResult Execute(CommandContext ctx)
        {
            var value = int.Parse(ctx.Arguments[0].Value);

            ctx.Indentation += value;
            if (ctx.Indentation < 0)
                ctx.Indentation = 0;

            return new CommandResult(true);
        }
    }
}
