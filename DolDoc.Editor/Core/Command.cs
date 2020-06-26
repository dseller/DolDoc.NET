using System.Collections.Generic;

namespace DolDoc.Editor.Core
{
    public class Command
    {
        public string Mnemonic { get; }

        public IList<Flag> Flags { get; }

        public IList<Argument> Arguments { get; }

        public int TextOffset { get; }

        public Command(int textOffset, string mnemonic, IList<Flag> flags, IList<Argument> args)
        {
            TextOffset = textOffset;
            Mnemonic = mnemonic;
            Flags = flags;
            Arguments = args;
        }

        public static Command CreateTextCommand(int textOffset, IList<Flag> flags, string value) =>
            new Command(textOffset, "TX", flags, new[] { new Argument(null, value) });
    }
}
