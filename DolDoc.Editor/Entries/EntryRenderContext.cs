using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class EntryRenderContext
    {
        /// <summary>
        /// The current Document.
        /// </summary>
        public Document Document { get; set; }

        public ViewerState State { get; set; }

        /// <summary>
        /// The position where to render the entry.
        /// </summary>
        public int RenderPosition { get; set; }

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
    }
}
