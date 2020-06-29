using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public IList<Flag> Flags { get; }

        public IList<Argument> Arguments { get; }

        /// <summary>
        /// The position of this document entry in the editor's raw text buffer.
        /// </summary>
        public int TextOffset { get; }

        public DocumentEntry(DocumentCommand cmd, int textOffset, IList<Flag> flags, IList<Argument> args)
        {
            Flags = flags;
            Command = cmd;
            Arguments = args;
            TextOffset = textOffset;
        }

        public static DocumentEntry CreateTextCommand(int textOffset, IList<Flag> flags, string value) =>
            new DocumentEntry(DocumentCommand.Text, textOffset, flags, new[] { new Argument(null, value) });

        public bool HasFlag(string flag) => Flags.Any(f => f.Value == flag && f.Status);

        public void CharKeyPress(ViewerState state, char key, int relativeOffset)
        {
            Arguments[0].Value = Arguments[0].Value.Insert(relativeOffset, new string(key, 1));
            state.CursorPosition++;
        }

        public void KeyPress(ViewerState state, ConsoleKey key, int relativeOffset)
        {
            switch (key)
            {
                case ConsoleKey.Delete:
                    Arguments[0].Value = Arguments[0].Value.Remove(relativeOffset, 1);
                    break;

                case ConsoleKey.Backspace:
                    if (relativeOffset > 0)
                        Arguments[0].Value = Arguments[0].Value.Remove(relativeOffset - 1, 1);
                    state.CursorPosition--;
                    break;
            }
        }
    }
}
