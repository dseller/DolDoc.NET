using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Core
{
    public class CommandResult
    {
        public bool Success { get; }

        public int WrittenCharacters { get; }

        public CommandResult(bool success, int writtenCharacters = 0)
        {
            Success = success;
            WrittenCharacters = writtenCharacters;
        }
    }
}
