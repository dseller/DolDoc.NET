using System.Collections.Generic;
using System.Linq;

namespace DolDoc.Core.Editor
{
    public class DolDocInstruction<T>
    {
        public string[] Flags { get; }

        public T Data { get; }

        public DolDocInstruction(IEnumerable<string> flags, T data)
        {
            Flags = flags?.ToArray() ?? new string[0];
            Data = data;
        }
    }
}
