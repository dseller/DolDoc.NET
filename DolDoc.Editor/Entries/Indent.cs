using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Entries
{
    public class Indent : DocumentEntry
    {
        public Indent(IList<Flag> flags, IList<Argument> args) 
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var value = int.Parse(Tag);

            ctx.Indentation += value;
            if (ctx.Indentation < 0)
                ctx.Indentation = 0;

            if (ctx.CollapsedTreeNodeIndentationLevel.HasValue && 
                ctx.CollapsedTreeNodeIndentationLevel.Value == ctx.Indentation)
                    ctx.CollapsedTreeNodeIndentationLevel = null;

            return new CommandResult(true);
        }

        public override string ToString() => AsString("ID");
    }
}
