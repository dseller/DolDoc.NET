using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using Microsoft.Extensions.Logging;

namespace DolDoc.Editor.Entries
{
    [Entry("ER")]
    public class Error : DocumentEntry
    {
        private readonly string errorMessage;

        public Error(string errorMessage) 
            : base(null, null)
        {
            this.errorMessage = errorMessage;
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var logger = LogSingleton.Instance.CreateLogger<Error>();
            logger.LogError("Error on position {0}: {1}", ctx.RenderPosition, errorMessage);
            return new CommandResult(true);
        }

        public override string ToString() => AsString("ER");
    }
}
