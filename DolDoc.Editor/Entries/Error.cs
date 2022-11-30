using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    [Entry("ER")]
    public class Error : DocumentEntry
    {
        private readonly string _errorMessage;

        public Error(string errorMsg) 
            : base(new List<Flag>(), new List<Argument>() { new Argument(null, errorMsg) })
        {
            _errorMessage = errorMsg;
        }

        public Error(IList<Flag> flags, IList<Argument> args) : base(flags, args)
        {
            _errorMessage = Tag;
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            if (_errorMessage == null)
                return new CommandResult(true);
            
            ctx.PushOptions(new RenderOptions
            {
                BackgroundColor = EgaColor.Red,
                ForegroundColor = EgaColor.White,
                // Blink = true
            });

            var written = WriteString(ctx, _errorMessage);

            ctx.PopOptions();

            return new CommandResult(true, written);
        }

        public override string ToString() => AsString("ER");
    }
}
