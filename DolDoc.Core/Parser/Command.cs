using System.Collections.Generic;

namespace DolDoc.Core.Parser
{
    public class Command : DocumentNode
    {
        public Command(string code, IEnumerable<Flag> flags, IEnumerable<Argument> arguments)
        {
            Code = code;
            Flags = flags;
            Arguments = arguments;
        }

        public string Code { get; }

        public IEnumerable<Flag> Flags { get; }

        public IEnumerable<Argument> Arguments { get; }

        public override string GetInfo() => "Command: " + Code;
    }
}
