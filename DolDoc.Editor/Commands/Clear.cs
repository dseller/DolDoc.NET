using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class Clear : IDolDocCommand
    {
        public CommandResult Execute(CommandContext ctx)
        {
            ctx.State.Pages.Clear(ctx.BackgroundColor);

            return new CommandResult(true);
        }
    }
}
