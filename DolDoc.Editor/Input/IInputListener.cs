using DolDoc.Editor.Core;
using DolDoc.Editor.Rendering;

namespace DolDoc.Editor.Input
{
    public interface IInputListener
    {
        void KeyPress(Key key);

        void MouseDown(float x, float y);

        CursorType MouseMove(float x, float y);
    }
}
