using DolDoc.Editor.Input;
using DolDoc.Editor.Rendering;
using DolDoc.Editor.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor
{
    public class EditorState : IInputListener
    {
        private readonly TextBuffer _textualRepresentation;
        private readonly Composer _composer;
        private readonly CharacterMatrix _visualRepresentation;

        public EditorState()
        {

        }

        public void KeyDown(ConsoleKey key)
        {
            throw new NotImplementedException();
        }

        public void KeyPress(ConsoleKey key)
        {
            throw new NotImplementedException();
        }

        public void KeyUp(ConsoleKey key)
        {
            throw new NotImplementedException();
        }

        public void MousePress(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void MouseMove(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void MouseRelease(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
