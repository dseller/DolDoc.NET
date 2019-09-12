using DolDoc.Editor.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.UI
{
    public abstract class Control : IRenderable
    {
        public abstract void OnClick();
    }
}
