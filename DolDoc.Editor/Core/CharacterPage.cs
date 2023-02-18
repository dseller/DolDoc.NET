using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DolDoc.Editor.Core
{
    public class CharacterPage : IEnumerable<Character>
    {
        private readonly Character[] characters;

        public int PageNumber { get; }

        public int Columns { get; }

        public int Rows { get; }

        public CharacterPage(ViewerState viewerState, int pageNo, int columns, int rows)
        {
            Rows = rows;
            Columns = columns;
            PageNumber = pageNo;
            characters = new Character[rows * columns];
            for (int i = 0; i < characters.Length; i++)
                characters[i] = new Character((pageNo * columns * rows) + i);

            Clear(viewerState.DefaultBackgroundColor);
        }

        public Character this[int pos]
        {
            get => characters[pos];
            set => characters[pos] = value;
        }

        public Character this[int x, int y]
        {
            get => characters[(y * Columns) + x];
            set => characters[(y * Columns) + x] = value;
        }

        public void Clear(EgaColor color)
        {
            for (int i = 0; i < characters.Length; i++)
                if ((characters[i].Flags & CharacterFlags.Hold) == 0)
                    characters[i].Write(null, i, (byte)0x00, new CombinedColor(color, color));
        }

        public IEnumerator<Character> GetEnumerator() => characters.AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => characters.GetEnumerator();
    }
}
