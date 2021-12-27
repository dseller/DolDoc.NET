namespace DolDoc.Editor.Core
{
    public class RenderOptions
    {
        public RenderOptions(EgaColor defaultFgColor = EgaColor.Black, EgaColor defaultBgColor = EgaColor.White)
        {
            DefaultBackgroundColor = defaultBgColor;
            DefaultForegroundColor = defaultFgColor;
            BackgroundColor = defaultBgColor;
            ForegroundColor = defaultFgColor;
        }

        public EgaColor ForegroundColor { get; set; }

        public EgaColor BackgroundColor { get; set; }

        public EgaColor DefaultForegroundColor { get; set; }

        public EgaColor DefaultBackgroundColor { get; set; }

        /// <summary>
        /// Whether underline mode is enabled.
        /// </summary>
        public bool Underline { get; set; }

        /// <summary>
        /// Whether blinking mode is enabled.
        /// </summary>
        public bool Blink { get; set; }

        public bool WordWrap { get; set; }

        public bool Inverted { get; set; }

        /// <summary>
        /// Defines the indentation level.
        /// </summary>
        public int Indentation { get; set; }

        public int? CollapsedTreeNodeIndentationLevel { get; set; }

        public RenderOptions Clone() => new RenderOptions(DefaultForegroundColor, DefaultBackgroundColor)
        {
            Underline = Underline,
            Blink = Blink,
            WordWrap = WordWrap,
            Inverted = Inverted,
            Indentation = Indentation,
            CollapsedTreeNodeIndentationLevel = CollapsedTreeNodeIndentationLevel
        };
    }
}
