using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;

namespace DolDoc.Editor.Input
{
    public class InputManager : IInputListener
    {
        private readonly IEnumerable<IInputListener> _inputListeners;

        public InputManager(IEnumerable<IInputListener> inputListeners)
        {
            _inputListeners = inputListeners;
        }

        public void KeyPress(Key key)
        {
            foreach (var listener in _inputListeners)
                listener.KeyPress(key);
        }

        public void MouseMove(int x, int y)
        {
            foreach (var listener in _inputListeners)
                listener.MouseMove(x, y);
        }

        public void MousePress(int x, int y)
        {
            foreach (var listener in _inputListeners)
                listener.MousePress(x, y);
        }

        public void MouseRelease(int x, int y)
        {
            foreach (var listener in _inputListeners)
                listener.MouseRelease(x, y);
        }
    }
}
