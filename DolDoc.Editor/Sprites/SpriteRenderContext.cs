using DolDoc.Editor.Core;

namespace DolDoc.Editor.Sprites
{
    public class SpriteRenderContext
    {
        public SpriteRenderContext(ViewerState state)
        {
            State = state;
        }

        public int Thickness { get; set; }

        public EgaColor Color { get; set; }

        public ViewerState State { get; }

    }
}
