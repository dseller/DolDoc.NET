using DolDoc.Editor.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Core
{
    /// <summary>
    /// A class representing the cursor of the document.
    /// </summary>
    public class Cursor
    {
        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        private readonly ViewerState _viewerState;

        public Cursor(ViewerState viewerState)
        {
            _viewerState = viewerState;
        }

        /// <summary>
        /// The column of the cursor in the window (not adjusted to the ViewLine)
        /// </summary>
        public int WindowX => DocumentX;

        /// <summary>
        /// The row of the cursor in the window (not adjusted to the ViewLine)
        /// </summary>
        public int WindowY => (DocumentY - ViewLine).Clamp(0, _viewerState.Rows);

        public int ViewLine { get; private set; }

        /// <summary>
        /// The selected column within the document
        /// </summary>
        public int DocumentX => DocumentPosition % _viewerState.Columns;

        /// <summary>
        /// The selected row within the document
        /// </summary>
        public int DocumentY => DocumentPosition / _viewerState.Columns;

        /// <summary>
        /// The index of the cursor in the document
        /// </summary>
        public int DocumentPosition { get; set; }

        /// <summary>
        /// Get the <seealso cref="DocumentEntry"/> that the cursor is currently selecting.
        /// </summary>
        public DocumentEntry SelectedEntry => _viewerState.Pages[DocumentPosition].Entry;

        /// <summary>
        /// Advances the cursor <paramref name="characters"/> amount of chars.
        /// </summary>
        /// <param name="characters">Amount of characters to advance.</param>
        public void Right()
        {
            if (_viewerState.Pages[DocumentPosition + 1].HasEntry)
                DocumentPosition++;
            else
                DocumentPosition = FindNearestEntry(Direction.Right) ?? DocumentPosition;

            if (DocumentY > ViewLine + _viewerState.Rows)
                ViewLine += (DocumentY - (ViewLine + _viewerState.Rows));
        }

        /// <summary>
        /// Rewinds the cursor <paramref name="characters"/> amount of chars.
        /// </summary>
        /// <param name="characters"></param>
        public void Left()
        {
            if (DocumentPosition == 0)
                return;

            if (_viewerState.Pages[DocumentPosition - 1].HasEntry)
                DocumentPosition--;
            else
                DocumentPosition = FindNearestEntry(Direction.Left) ?? DocumentPosition;
        }

        public void Up()
        {
            if (DocumentPosition < _viewerState.Columns)
                return;

            if (_viewerState.Pages[DocumentPosition - _viewerState.Columns].HasEntry)
                DocumentPosition -= _viewerState.Columns;
            else
                DocumentPosition = FindNearestEntry(Direction.Up) ?? DocumentPosition;

            if (DocumentY < ViewLine)
                ViewLine -= ViewLine - DocumentY;
        }

        public void Down()
        {
            if (!_viewerState.Pages.HasPageForPosition(DocumentPosition + _viewerState.Columns))
                return;

            if (_viewerState.Pages[DocumentPosition + _viewerState.Columns].HasEntry)
                DocumentPosition += _viewerState.Columns;
            else
                DocumentPosition = FindNearestEntry(Direction.Down) ?? DocumentPosition;

            if (DocumentY >= ViewLine + _viewerState.Rows)
                ViewLine += (DocumentY - (ViewLine + _viewerState.Rows)) + 1;
        }

        /// <summary>
        /// Scans the document in the specified direction <paramref name="dir"/>, and returns the position of the nearest <seealso cref="DocumentEntry"/>.
        /// If no entry was found, returns null.
        /// </summary>
        /// <param name="dir">The direction to scan</param>
        /// <returns>The position, or null if no entry was found</returns>
        private int? FindNearestEntry(Direction dir)
        {
            int position = DocumentPosition;

            if (dir == Direction.Up)
            {
                position -= (position % _viewerState.Columns);
                if (position < 0)
                    return null;
            }
            else if (dir == Direction.Down)
                position += (_viewerState.Columns - (position % _viewerState.Columns));

            do
            {
                switch (dir)
                {
                    case Direction.Up:
                    case Direction.Left:
                        position--;
                        break;

                    case Direction.Down:
                    case Direction.Right:
                        position++;
                        break;
                }

                // If there is no page for the requested position, return null, since we
                // could not find a entry for it.
                if (!_viewerState.Pages.HasPageForPosition(position))
                    return null;
            } while (!_viewerState.Pages[position].HasEntry);

            return position;
        }
    }
}
