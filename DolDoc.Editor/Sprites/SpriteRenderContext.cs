using DolDoc.Editor.Core;

namespace DolDoc.Editor.Sprites
{
    public class SpriteRenderContext
    {
        public SpriteRenderContext(Compositor.Compositor compositor, ViewerState state)
        {
            State = state;
            Compositor = compositor;
        }

        public int Thickness { get; set; }

        public EgaColor Color { get; set; }

        public ViewerState State { get; }
        
        public Compositor.Compositor Compositor { get; }

    }
}
