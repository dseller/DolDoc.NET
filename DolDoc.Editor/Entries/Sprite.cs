using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System.Collections.Generic;
using System.Linq;

namespace DolDoc.Editor.Entries
{
    [Entry("SP")]
    public class Sprite : DocumentEntry
    {
        public Sprites.Sprite SpriteObj { get; private set; }

        public int SpriteOffset { get; private set; }

        public Sprite(IList<Flag> flags, IList<Argument> args) 
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            var binaryIndex = GetArgument("BI");
            if (binaryIndex == null || !int.TryParse(binaryIndex, out var id) || ctx.Document.BinaryChunks == null)
                return new CommandResult(false);

            var chunk = ctx.Document.BinaryChunks.FirstOrDefault(bc => bc.Id == id);
            if (chunk == null)
                return new CommandResult(false);

            int writtenChars = 0;
            if (Tag != null)
            {
                writtenChars = WriteString(ctx, Tag);
            }

            SpriteObj = new Sprites.Sprite(chunk.Data);
            SpriteOffset = ctx.RenderPosition + writtenChars;

            return new CommandResult(true, writtenChars);
        }

        public override string ToString() => AsString("SP");
    }
}
