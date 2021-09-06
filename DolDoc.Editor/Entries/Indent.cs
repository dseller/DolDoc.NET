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

            ctx.Options.Indentation += value;
            if (ctx.Options.Indentation < 0)
                ctx.Options.Indentation = 0;

            if (ctx.Options.CollapsedTreeNodeIndentationLevel.HasValue && 
                ctx.Options.CollapsedTreeNodeIndentationLevel.Value == ctx.Options.Indentation)
                    ctx.Options.CollapsedTreeNodeIndentationLevel = null;

            return new CommandResult(true);
        }

        public override string ToString() => AsString("ID");
    }
}
