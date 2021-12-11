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
                var page = GetPageForPosition(position % PageColumns, position / PageColumns);
                if (page == null)
                    return default;
                return page[position % (page.Columns * page.Rows)];
            }

            set
            {
                var page = GetOrCreatePageForPosition(position % PageColumns, position / PageColumns);
                page[position % (page.Columns * page.Rows)] = value;
            }
        }

        public Character this[int x, int y]
        {
            get => this[(y * PageColumns) + x];
            set => this[(y * PageColumns) + x] = value;
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

        public void Clear(EgaColor color = EgaColor.White)
        {
            foreach (var page in _pages)
                page.Clear(color);
        }

        public bool HasPageForPosition(int position)
        {
            int pageIndex = position / PageRows / PageColumns;
            return pageIndex < _pages.Count;
        }

        public bool HasPageForPosition(int x, int y)
        {
            int pageIndex = y / PageRows;
            return pageIndex < _pages.Count;
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

        private CharacterPage GetPageForPosition(int x, int y)
        {
            int pageIndex = y / PageRows;

            if (pageIndex < 0 || pageIndex >= _pages.Count)
                return null;

            return _pages[pageIndex];
        }
    }
}
