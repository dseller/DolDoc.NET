using DolDoc.Editor.Commands;
using DolDoc.Editor.Entries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DolDoc.Editor.Core
{
    public abstract class DocumentEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<Flag> Flags { get; }

        public IList<Argument> Arguments { get; }

        /// <summary>
        /// The position of this document entry in the editor's raw text buffer.
        /// </summary>
        public int TextOffset { get; }

        public string Tag => Arguments.FirstOrDefault(arg => arg.Key == "T" || arg.Key == null)?.Value;

        public DocumentEntry(int textOffset, IList<Flag> flags, IList<Argument> args)
        {
            Flags = flags;
            Arguments = args;
            TextOffset = textOffset;
        }

        public static DocumentEntry CreateTextCommand(int textOffset, IList<Flag> flags, string value) =>
            new Text(textOffset, flags, new[] { new Argument(null, value) });

        public bool HasFlag(string flag) => Flags.Any(f => f.Value == flag && f.Status);

        public virtual void CharKeyPress(ViewerState state, char key, int relativeOffset)
        {
            Arguments[0].Value = Arguments[0].Value.Insert(relativeOffset, new string(key, 1));
            state.CursorPosition++;
        }

        public virtual void KeyPress(ViewerState state, ConsoleKey key, int relativeOffset)
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

        public abstract CommandResult Evaluate(EntryRenderContext ctx);

        public abstract override string ToString();
    }
}
