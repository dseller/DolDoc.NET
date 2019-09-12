using System.Collections.Generic;

namespace DolDoc.Interpreter.Domain
{
    public class Command
    {
        public string Mnemonic { get; }

        public IEnumerable<Flag> Flags { get; }

        public IEnumerable<Argument> Arguments { get; }

        public Command(string mnemonic, IEnumerable<Flag> flags, IEnumerable<Argument> args)
        {
            Mnemonic = mnemonic;
            Flags = flags;
            Arguments = args;
        }
    }
}
