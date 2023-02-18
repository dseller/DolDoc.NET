using System.Collections.Generic;
using DolDoc.Editor.Core;

namespace DolDoc.Editor.Parser
{
    public interface IDolDocParser
    {
        IEnumerable<DocumentEntry> Parse(string input);
    }
}
