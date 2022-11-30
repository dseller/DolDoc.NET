using System;
using System.Collections.Generic;

namespace DolDoc.Editor.Core
{
    public class CharacterPageDirectory
    {
        private readonly ViewerState viewerState;
        private readonly List<CharacterPage> pages;

        public int PageColumns { get; }

        public int PageRows { get; }

        public int PageCount => pages.Count;

        public CharacterPageDirectory(ViewerState viewerState, int pageColumns, int pageRows)
        {
            this.viewerState = viewerState;
            PageRows = pageRows;
            PageColumns = pageColumns;
            pages = new List<CharacterPage>();
            pages.Add(new CharacterPage(viewerState, 1, PageColumns, PageRows));
        }

        public Character this[int position]
        {
            get 
            {
                var page = GetOrCreatePageForPosition(position % PageColumns, position / PageColumns);
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

        public CharacterPage Get(int pageNumber) => pages[pageNumber];

        public CharacterPage GetOrCreatePage(int x, int y)
        {
            return GetOrCreatePageForPosition(x, y);
        }

        public CharacterPage GetOrCreatePage(int position)
        {
            return GetOrCreatePage(position % PageColumns, position / PageRows);
        }

        public void Clear(EgaColor color)
        {
            foreach (var page in pages)
                page.Clear(color);
        }

        public bool HasPageForPosition(int position)
        {
            int pageIndex = position / PageRows / PageColumns;
            return pageIndex < pages.Count;
        }

        public bool HasPageForPosition(int x, int y)
        {
            int pageIndex = y / PageRows;
            return pageIndex < pages.Count;
        }

        private CharacterPage GetOrCreatePageForPosition(int x, int y)
        {
            int pageIndex = y / PageRows;

            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(y));

            if (pageIndex >= pages.Count)
            {
                // Create a (series of) new page(s).
                int originalPages = PageCount;
                int pagesToCreate = pageIndex - pages.Count + 1;
                for (int i = 1; i <= pagesToCreate; i++)
                    pages.Add(new CharacterPage(viewerState, originalPages + i, PageColumns, PageRows));
            }

            return pages[pageIndex];
        }

        private CharacterPage GetPageForPosition(int x, int y)
        {
            int pageIndex = y / PageRows;

            if (pageIndex < 0 || pageIndex >= pages.Count)
                return null;

            return pages[pageIndex];
        }
    }
}
