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
        public int WindowX { get; set; }

        /// <summary>
        /// The row of the cursor in the window (not adjusted to the ViewLine)
        /// </summary>
        public int WindowY { get; set; }

        /// <summary>
        /// The index of the cursor in the window (not adjusted to the ViewLine)
        /// </summary>
        public int WindowPosition { get; set; }

        /// <summary>
        /// The selected column within the document (adjusted to the ViewLine)
        /// </summary>
        public int DocumentX { get; set; }

        /// <summary>
        /// The selected row within the document (adjusted to the ViewLine)
        /// </summary>
        public int DocumentY { get; set; }

        /// <summary>
        /// The index of the cursor in the document (adjusted to the ViewLine)
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
            {
                var position = FindNearestEntry(Direction.Right);
                if (position != null)
                    DocumentPosition = position.Value;
            }
        }

        /// <summary>
        /// Rewinds the cursor <paramref name="characters"/> amount of chars.
        /// </summary>
        /// <param name="characters"></param>
        public void Left()
        {

        }

        public void Up()
        {

        }

        public void Down()
        {

        }

        /// <summary>
        /// Scans the document in the specified direction <paramref name="dir"/>, and returns the position of the nearest <seealso cref="DocumentEntry"/>.
        /// If no entry was found, returns null.
        /// </summary>
        /// <param name="dir">The direction to scan</param>
        /// <returns>The position, or null if no entry was found</returns>
        private int? FindNearestEntry(Direction dir, int? offset = null)
        {
            int position = DocumentPosition + (offset ?? 0);

            do
            {
                switch (dir)
                {
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
