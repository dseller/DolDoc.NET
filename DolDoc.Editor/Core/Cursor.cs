// <copyright file="Cursor.cs" company="Dennis Seller">
// Copyright (c) Dennis Seller. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DolDoc.Editor.Extensions;

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
            Down,
        }

        private readonly ViewerState viewerState;

        public Cursor(ViewerState viewerState)
        {
            this.viewerState = viewerState;
        }

        /// <summary>
        /// Gets the column of the cursor in the window (not adjusted to the ViewLine).
        /// </summary>
        public int WindowX => DocumentX;

        /// <summary>
        /// Gets the row of the cursor in the window (not adjusted to the ViewLine).
        /// </summary>
        public int WindowY => (DocumentY - ViewLine).Clamp(0, viewerState.Rows);

        public int ViewLine { get; private set; }

        /// <summary>
        /// Gets the selected column within the document.
        /// </summary>
        public int DocumentX => DocumentPosition % viewerState.Columns;

        /// <summary>
        /// Gets the selected row within the document.
        /// </summary>
        public int DocumentY => DocumentPosition / viewerState.Columns;

        /// <summary>
        /// Gets or sets the index of the cursor in the document.
        /// </summary>
        public int DocumentPosition { get; set; }

        /// <summary>
        /// Gets the <seealso cref="DocumentEntry"/> that the cursor is currently selecting.
        /// </summary>
        public DocumentEntry SelectedEntry => viewerState.Pages[DocumentPosition].Entry;

        /// <summary>
        /// Advances the cursor <paramref name="characters"/> amount of chars.
        /// </summary>
        /// <param name="characters">Amount of characters to advance.</param>
        public void Right()
        {
            var currentlySelectedEntry = SelectedEntry;
            if (viewerState.Pages[DocumentPosition + 1].HasEntry)
                DocumentPosition++;
            else
                DocumentPosition = FindNearestEntry(Direction.Right) ?? DocumentPosition;

            if (DocumentY > ViewLine + viewerState.Rows)
                ViewLine += DocumentY - (ViewLine + viewerState.Rows);

            if (currentlySelectedEntry != null)
                currentlySelectedEntry.Selected = false;
            SelectedEntry.Selected = true;
        }

        /// <summary>
        /// Rewinds the cursor <paramref name="characters"/> amount of chars.
        /// </summary>
        public void Left()
        {
            if (DocumentPosition == 0)
                return;

            var currentlySelectedEntry = SelectedEntry;
            if (viewerState.Pages[DocumentPosition - 1].HasEntry)
                DocumentPosition--;
            else
                DocumentPosition = FindNearestEntry(Direction.Left) ?? DocumentPosition;

            if (currentlySelectedEntry != null)
                currentlySelectedEntry.Selected = false;
            SelectedEntry.Selected = true;
        }

        public void Up()
        {
            if (DocumentPosition < viewerState.Columns)
                return;

            var currentlySelectedEntry = SelectedEntry;
            if (viewerState.Pages[DocumentPosition - viewerState.Columns].HasEntry)
                DocumentPosition -= viewerState.Columns;
            else
                DocumentPosition = FindNearestEntry(Direction.Up) ?? DocumentPosition;

            if (DocumentY < ViewLine)
                ViewLine -= ViewLine - DocumentY;

            if (currentlySelectedEntry != null)
                currentlySelectedEntry.Selected = false;
            SelectedEntry.Selected = true;
        }

        public void Down()
        {
            if (!viewerState.Pages.HasPageForPosition(DocumentPosition + viewerState.Columns))
                return;

            var currentlySelectedEntry = SelectedEntry;
            if (viewerState.Pages[DocumentPosition + viewerState.Columns].HasEntry)
                DocumentPosition += viewerState.Columns;
            else
                DocumentPosition = FindNearestEntry(Direction.Down) ?? DocumentPosition;

            if (DocumentY >= ViewLine + viewerState.Rows)
                ViewLine += (DocumentY - (ViewLine + viewerState.Rows)) + 1;

            if (currentlySelectedEntry != null)
                currentlySelectedEntry.Selected = false;
            SelectedEntry.Selected = true;
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
                position -= position % viewerState.Columns;
                if (position < 0)
                    return null;
            }
            else if (dir == Direction.Down)
            {
                position += viewerState.Columns - (position % viewerState.Columns);
            }

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
                if (!viewerState.Pages.HasPageForPosition(position))
                    return null;
            } while (!viewerState.Pages[position].HasEntry);

            return position;
        }
    }
}
