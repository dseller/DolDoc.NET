using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;

namespace DolDoc.Editor.Entries
{
    public class Error : DocumentEntry
    {
        private string _errorMessage;

        public Error(int textOffset, string errorMsg) 
            : base(textOffset, null, null)
        {
            _errorMessage = errorMsg;
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            throw new Exception(_errorMessage);
        }

        public override string ToString() => "$ER$";
    }
}
