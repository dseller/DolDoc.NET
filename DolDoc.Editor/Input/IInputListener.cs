using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Input
{
    public interface IInputListener
    {
        void KeyDown(ConsoleKey key);

        void KeyUp(ConsoleKey key);

        void KeyPress(ConsoleKey key);

        void MousePress(int x, int y);

        void MouseMove(int x, int y);

        void MouseRelease(int x, int y);
    }
}
