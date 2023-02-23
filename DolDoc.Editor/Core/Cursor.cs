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
            Down
        }

        private readonly ViewerState viewerState;

        public Cursor(ViewerState viewerState)
        {
            this.viewerState = viewerState;
        }

        /// <summary>
        /// The column of the cursor in the window (not adjusted to the ViewLine)
        /// </summary>
        public int WindowX => DocumentX;

        /// <summary>
        /// The row of the cursor in the window (not adjusted to the ViewLine)
        /// </summary>
        public int WindowY => (DocumentY - ViewLine).Clamp(0, viewerState.Rows - 1);

        /// <summary>
        /// Gets or sets the offset at which the viewer's top line is placed in the document.
        /// </summary>
        public int ViewLine { get; private set; }

        /// <summary>
        /// The selected column within the document
        /// </summary>
        public int DocumentX => DocumentPosition % viewerState.Columns;

        /// <summary>
        /// The selected row within the document
        /// </summary>
        public int DocumentY => DocumentPosition / viewerState.Columns;

        /// <summary>
        /// The index of the cursor in the document
        /// </summary>
        public int DocumentPosition { get; private set; }

        public void SetPosition(int position)
        {
            DocumentPosition = position;

            if (DocumentY > ViewLine + viewerState.Rows)
                ViewLine += (DocumentY - (ViewLine + viewerState.Rows)) + 1;
            else if (DocumentY < ViewLine)
                ViewLine -= ViewLine - DocumentY;
        }

        /// <summary>
        /// Get the <seealso cref="DocumentEntry"/> that the cursor is currently selecting.
        /// </summary>
        public DocumentEntry SelectedEntry => viewerState.Pages[DocumentPosition].Entry;

        /// <summary>
        /// Advances the cursor right.
        /// </summary>
        public void Right()
        {
            var currentlySelectedEntry = SelectedEntry;
            DocumentPosition++;
            if (DocumentY > ViewLine + viewerState.Rows)
                ViewLine += (DocumentY - (ViewLine + viewerState.Rows));

            if (currentlySelectedEntry != null)
                currentlySelectedEntry.Selected = false;
            if (SelectedEntry == null)
            {
                var targetEntry = FindNearestEntry(Direction.Right);
                if (targetEntry.HasValue)
                    DocumentPosition = targetEntry.Value;
            }
            if (SelectedEntry != null)
                SelectedEntry.Selected = true;
        }

        /// <summary>
        /// Rewinds the cursor <paramref name="characters"/> amount of chars.
        /// </summary>
        /// <param name="characters"></param>
        public void Left()
        {
            if (DocumentPosition == 0)
                return;

            var currentlySelectedEntry = SelectedEntry;
            DocumentPosition--;

            if (currentlySelectedEntry != null)
                currentlySelectedEntry.Selected = false;
            if (SelectedEntry == null)
            {
                var targetEntry = FindNearestEntry(Direction.Left);
                if (targetEntry.HasValue)
                    DocumentPosition = targetEntry.Value;
            }
            if (SelectedEntry != null)
                SelectedEntry.Selected = true;
        }

        public void Up()
        {
            if (DocumentPosition < viewerState.Columns)
                return;

            var currentlySelectedEntry = SelectedEntry;
            DocumentPosition -= viewerState.Columns;

            if (DocumentY < ViewLine)
                ViewLine -= ViewLine - DocumentY;

            if (currentlySelectedEntry != null)
                currentlySelectedEntry.Selected = false;

            if (SelectedEntry == null)
            {
                var targetEntry = FindNearestEntry(Direction.Left);
                if (targetEntry.HasValue)
                    DocumentPosition = targetEntry.Value;
            }
            
            if (SelectedEntry != null)
                SelectedEntry.Selected = true;
        }

        public void Down()
        {
            if (!viewerState.Pages.HasPageForPosition(DocumentPosition + viewerState.Columns))
                return;

            var currentlySelectedEntry = SelectedEntry;
            DocumentPosition += viewerState.Columns;

            if (DocumentY >= ViewLine + viewerState.Rows)
                ViewLine += (DocumentY - (ViewLine + viewerState.Rows)) + 1;

            if (currentlySelectedEntry != null)
                currentlySelectedEntry.Selected = false;
            if (SelectedEntry == null)
            {
                var targetEntry = FindNearestEntry(Direction.Left);
                if (targetEntry.HasValue)
                    DocumentPosition = targetEntry.Value;
            }
            if (SelectedEntry != null)
                SelectedEntry.Selected = true;
        }

        public void PageDown()
        {
            if (!viewerState.Pages.HasPageForPosition(DocumentPosition + viewerState.Rows * viewerState.Columns))
                return;

            ViewLine += viewerState.Rows;
            DocumentPosition += viewerState.Rows * viewerState.Columns;
        }

        public void PageUp()
        {
            ViewLine -= viewerState.Rows;
            DocumentPosition -= viewerState.Rows * viewerState.Columns;

            if (ViewLine < 0)
                ViewLine = 0;
            if (DocumentPosition < 0)
                DocumentPosition = 0;
        }

        public void Move(int oldX, int oldY, int newX, int newY)
        {
            // TODO
            viewerState.Pages[DocumentPosition].Flags ^= CharacterFlags.Inverted;
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
                position -= (position % viewerState.Columns);
                if (position < 0)
                    return null;
            }
            else if (dir == Direction.Down)
                position += (viewerState.Columns - (position % viewerState.Columns));

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
