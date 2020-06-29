using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DolDoc.Editor.Commands
{
    public class CommandContext
    {
        public Document Document { get; set; }

        public ViewerState State { get; set; }

        public int TextOffset { get; set; }

        public int RenderPosition { get; set; }

        //public IList<Flag> Flags { get; set; }
        //public IList<Argument> Arguments { get; set; }

        public EgaColor ForegroundColor { get; set; }

        public EgaColor BackgroundColor { get; set; }

        public EgaColor DefaultForegroundColor { get; set; }

        public EgaColor DefaultBackgroundColor { get; set; }

        public bool Underline { get; set; }

        public bool Blink { get; set; }

        public bool WordWrap { get; set; }

        public bool Inverted { get; set; }

        public int Indentation { get; set; }

        // public DocumentEntry Entry { get; set; }

        /*public bool HasFlag(string flag) =>
            Entry.Flags.Any(f => f.Value == flag && f.Status);*/
        
    }
}
