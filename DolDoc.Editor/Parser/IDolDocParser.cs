using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Core.Parser
{
    public interface IDolDocParser
    {
        IEnumerable<DocumentEntry> Parse(string input);
    }
}
