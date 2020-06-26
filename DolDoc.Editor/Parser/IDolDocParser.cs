using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Core.Parser
{
    public interface IDolDocParser
    {
        IEnumerable<Command> Parse(string input);
    }
}
