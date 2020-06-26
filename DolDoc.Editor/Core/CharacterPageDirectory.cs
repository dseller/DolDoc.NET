using System;
using System.Collections.Generic;

namespace DolDoc.Editor.Core
{
    public class CharacterPageDirectory
    {
        private List<CharacterPage> _pages;

        public int PageColumns { get; }

        public int PageRows { get; }

        public int PageCount => _pages.Count;

        public CharacterPageDirectory(int pageColumns, int pageRows)
        {
            PageRows = pageRows;
            PageColumns = pageColumns;
            _pages = new List<CharacterPage>();
            _pages.Add(new CharacterPage(1, PageColumns, PageRows));
        }

        public Character this[int position]
        {
            get 
            {
                var page = GetOrCreatePageForPosition(position % PageColumns, position / PageRows);
                return page[position % (page.Columns * page.Rows)];
            }

            set
            {
                var page = GetOrCreatePageForPosition(position % PageColumns, position / PageRows);
                // page[((page.PageNumber - 1) * page.Columns * page.Rows) - position] = value;
                page[position % (page.Columns * page.Rows)] = value;
            }
        }

        public Character this[int x, int y]
        {
            get
            {
                var page = GetOrCreatePageForPosition(x, y);
                return page[x, y - ((page.PageNumber - 1) * page.Rows)];
            }

            set
            {
                var page = GetOrCreatePageForPosition(x, y);
                page[x, y - ((page.PageNumber - 1) * page.Rows)] = value;
            }
        }

        public CharacterPage Get(int pageNumber) => _pages[pageNumber];

        public CharacterPage GetOrCreatePage(int x, int y)
        {
            return GetOrCreatePageForPosition(x, y);
        }

        public CharacterPage GetOrCreatePage(int position)
        {
            return GetOrCreatePage(position % PageColumns, position / PageRows);
        }

        private CharacterPage GetOrCreatePageForPosition(int x, int y)
        {
            int pageIndex = y / PageRows;

            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(y));

            if (pageIndex >= _pages.Count)
            {
                // Create a (series of) new page(s).
                int originalPages = PageCount;
                int pagesToCreate = pageIndex - _pages.Count + 1;
                for (int i = 1; i <= pagesToCreate; i++)
                    _pages.Add(new CharacterPage(originalPages + i, PageColumns, PageRows));
            }

            return _pages[pageIndex];
        }
    }
}
