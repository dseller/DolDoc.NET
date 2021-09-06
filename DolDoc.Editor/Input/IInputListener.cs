using DolDoc.Editor.Core;

namespace DolDoc.Editor.Input
{
    public interface IInputListener
    {
        void KeyPress(Key key);

        void MousePress(int x, int y);

        void MouseMove(int x, int y);

        void MouseRelease(int x, int y);
    }
}
