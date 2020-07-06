using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;

namespace DolDoc.Editor.Entries
{
    public class Error : DocumentEntry
    {
        private string _errorMessage;

        public Error(string errorMsg) 
            : base(null, null)
        {
            _errorMessage = errorMsg;
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            Console.WriteLine("ERROR: " + _errorMessage);
            return new CommandResult(true);
        }

        public override string ToString() => "$ER$";
    }
}
