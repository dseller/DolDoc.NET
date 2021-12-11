using DolDoc.Editor.Core;

namespace DolDoc.Editor.Input
{
    public interface IInputListener
    {
        void KeyPress(Key key);

        void MouseClick(float x, float y);

        void MouseMove(float x, float y);
    }
}
