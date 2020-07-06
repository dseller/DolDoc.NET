using DolDoc.Editor.Core;
using System;

namespace DolDoc.Editor.Commands
{
    public class EntryRenderContext : ICloneable
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

        public object Clone() => new EntryRenderContext
            {
                State = State,
                Document = Document,
                RenderPosition = RenderPosition,
                ForegroundColor = ForegroundColor,
                BackgroundColor = BackgroundColor,
                DefaultBackgroundColor = DefaultBackgroundColor,
                DefaultForegroundColor = DefaultForegroundColor,
                Underline = Underline,
                Blink = Blink,
                WordWrap = WordWrap,
                Inverted = Inverted,
                Indentation = Indentation,
                CollapsedTreeNodeIndentationLevel = CollapsedTreeNodeIndentationLevel
            };
    }
}
