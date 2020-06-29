using System.Collections.Generic;
using System.Linq;

namespace DolDoc.Editor.Core
{
    public class DocumentEntry
    {
        /// <summary>
        /// The command for this entry.
        /// </summary>
        public DocumentCommand Command { get; }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<Flag> Flags { get; }

        public IReadOnlyList<Argument> Arguments { get; }

        /// <summary>
        /// The position of this document entry in the editor's raw text buffer.
        /// </summary>
        public int TextOffset { get; }

        public DocumentEntry(DocumentCommand cmd, int textOffset, IReadOnlyList<Flag> flags, IReadOnlyList<Argument> args)
        {
            Flags = flags;
            Command = cmd;
            Arguments = args;
            TextOffset = textOffset;
        }

        public static DocumentEntry CreateTextCommand(int textOffset, IReadOnlyList<Flag> flags, string value) =>
            new DocumentEntry(DocumentCommand.Text, textOffset, flags, new[] { new Argument(null, value) });

        public bool HasFlag(string flag) => Flags.Any(f => f.Value == flag && f.Status);
    }
}
