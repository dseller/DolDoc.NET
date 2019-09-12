using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.UI
{
    public class Window : Control
    {
        private readonly ICollection<Control> _controls;

        public string Title { get; set; }

        public void Maximize()
        {

        }

        public void Close()
        {

        }

        public override void OnClick()
        {
            throw new NotImplementedException();
        }
    }
}
