using DolDoc.Editor.Core;

namespace DolDoc.Editor.Commands
{
    public class EntryRenderContext
    {
        public Document Document { get; set; }

        public ViewerState State { get; set; }

        public int TextOffset { get; set; }

        public int RenderPosition { get; set; }

        public EgaColor ForegroundColor { get; set; }

        public EgaColor BackgroundColor { get; set; }

        public EgaColor DefaultForegroundColor { get; set; }

        public EgaColor DefaultBackgroundColor { get; set; }

        public bool Underline { get; set; }

        public bool Blink { get; set; }

        public bool WordWrap { get; set; }

        public bool Inverted { get; set; }

        public int Indentation { get; set; }
    }
}
