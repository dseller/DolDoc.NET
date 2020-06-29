﻿using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Blink : ContextSwitchEntryBase
    {
        public Blink(int textOffset, IList<Flag> flags, IList<Argument> args) 
            : base(textOffset, flags, args)
        {
        }

        public override string ToString() => $"$BK,{Tag}$";

        protected override void Set(EntryRenderContext ctx, bool status) => ctx.Blink = status;
    }
}
