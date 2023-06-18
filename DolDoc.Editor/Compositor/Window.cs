using System;
using DolDoc.Editor.Core;

namespace DolDoc.Editor.Compositor
{
    [Flags]
    public enum WindowFlags
    {
        None = 0x00,
        IsRoot = 0x01,
        HasBorder = 0x02,
        HasCloseButton = 0x04,
        HasFocus = 0x08
    }

    public class Window
    {
        private const int MaxTitleLength = 32; 
        private const WindowFlags DefaultFlags = WindowFlags.HasBorder | WindowFlags.HasCloseButton;

        private string title;
        private static readonly CombinedColor FocusedBorderColor = new CombinedColor(EgaColor.Blue, EgaColor.White);
        private static readonly CombinedColor BorderColor = new CombinedColor(EgaColor.LtGray, EgaColor.White);

        public Window(Compositor compositor, string title, int columns, int rows, int x, int y, Document doc = null, WindowFlags? windowFlags = null)
        {
            Flags = windowFlags ?? DefaultFlags;
            Title = title;
            Columns = columns;
            Rows = rows;
            Document = doc ?? new Document();
            X = x;
            Y = y;
            Compositor = compositor;
            State = new ViewerState(this, Document, HasBorder ? columns - 2 : columns, HasBorder ? rows - 2 : rows);
        }
        
        public WindowFlags Flags { get; set; }

        public bool HasBorder => (Flags & WindowFlags.HasBorder) == WindowFlags.HasBorder;
        public bool IsRoot => (Flags & WindowFlags.IsRoot) == WindowFlags.IsRoot;
        public bool HasCloseButton => (Flags & WindowFlags.HasCloseButton) == WindowFlags.HasCloseButton;
        public bool HasFocus => (Flags & WindowFlags.HasFocus) == WindowFlags.HasFocus;

        public int X { get; set; }

        public int Y { get; set; }

        public string Title
        {
            get => title;
            set => title = value;
        }

        public int Columns { get; }

        public int Rows { get; }

        public ViewerState State { get; }

        public Document Document { get; set; }

        public Compositor Compositor { get; }

        public Character GetCharacter(int x, int y)
        {
            if (HasBorder)
            {
                var borderColor = HasFocus ? FocusedBorderColor : BorderColor;
                
                if (y == 0 && HasCloseButton)
                {
                    // Render close button
                    if (x == Columns - 4)
                        return new Character((byte) '[', borderColor);
                    if (x == Columns - 3)
                        return new Character(Codepage437.Solar, borderColor);
                    if (x == Columns - 2)
                        return new Character((byte) ']', borderColor);
                }

                if (x == 0 && y == 0)
                    return new Character(Codepage437.SingleTopLeftCorner, borderColor);
                if (x == Columns - 1 && y == 0)
                    return new Character(Codepage437.SingleTopRightCorner, borderColor);
                if (y == 0)
                {
                    // Window title: 
                    if (x >= 1 && x < MaxTitleLength && !string.IsNullOrWhiteSpace(title) && (x - 1) < title.Length)
                        return new Character((byte)title[x - 1], borderColor);
                    return new Character(Codepage437.SingleHorizontalLine, borderColor);
                }

                if (x == 0 && y == Rows - 1)
                    return new Character(Codepage437.SingleBottomLeftCorner, borderColor);
                if (x == Columns - 1 && y == Rows - 1)
                    return new Character(Codepage437.SingleBottomRightCorner, borderColor);
                if (x == 0)
                    return new Character(Codepage437.SingleVerticalLine, borderColor);
                if (x == Columns - 1)
                {
                    if (State.Pages.PageCount > 1)
                    {
                        var lines = (float)State.Pages.PageRows * State.Pages.PageCount;
                        var positionInFile = (State.Cursor.DocumentY / lines);
                        var scrollBarIndex = positionInFile * (Rows - 4);
                        
                        if (y == 1)
                            return new Character(Codepage437.VerticalSingleHorizontalDouble, borderColor);
                        if (y == Rows - 2)
                            return new Character(Codepage437.VerticalSingleHorizontalDouble, borderColor);
                        if (y == Math.Floor(scrollBarIndex) + 2)
                            return new Character(Codepage437.SolidVerticalBlock, new CombinedColor(borderColor.Background, EgaColor.Red)); 
                        return new Character(Codepage437.SingleVerticalLine, borderColor);
                    }
                    else
                        return new Character(Codepage437.SingleVerticalLine, borderColor);
                }
                if (y == Rows - 1)
                    return new Character(Codepage437.SingleHorizontalLine, borderColor);

                if (!State.Pages.HasPageForPosition(x - 1, y - 1 + State.Cursor.ViewLine))
                    State.Pages.GetOrCreatePage(x - 1, y - 1 + State.Cursor.ViewLine);

                return State.Pages[x - 1, y - 1 + State.Cursor.ViewLine];
            }

            if (!State.Pages.HasPageForPosition(x, y + State.Cursor.ViewLine))
                State.Pages.GetOrCreatePage(x, y + State.Cursor.ViewLine);

            return State.Pages[x, y + State.Cursor.ViewLine];
        }
    }
}