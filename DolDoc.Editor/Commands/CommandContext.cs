using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DolDoc.Editor.Commands
{
    public class CommandContext
    {
        public ViewerState State { get; set; }

        public int TextOffset { get; set; }

        public int RenderPosition { get; set; }

        public IList<Flag> Flags { get; set; }

        public IList<Argument> Arguments { get; set; }

        public EgaColor ForegroundColor { get; set; }

        public EgaColor BackgroundColor { get; set; }

        public EgaColor DefaultForegroundColor { get; set; }

        public EgaColor DefaultBackgroundColor { get; set; }

        public bool Underline { get; set; }

        public bool HasFlag(string flag) =>
            Flags.Any(f => f.Value == flag && f.Status);
        
    }
}
