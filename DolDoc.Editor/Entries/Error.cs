using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    [Entry("ER")]
    public class Error : DocumentEntry
    {
        private string _errorMessage;

        public Error(string errorMsg) 
            : base(new List<Flag>(), new List<Argument>())
        {
            _errorMessage = errorMsg;
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
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

        public override string ToString() => "$ER$";
    }
}
