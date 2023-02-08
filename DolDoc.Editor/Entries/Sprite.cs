using System;
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
            var chunk = GetChunk(ctx);
            if (chunk == null)
                return new CommandResult(false);

            int writtenChars = 0;
            if (!string.IsNullOrEmpty(Tag))
                writtenChars = WriteString(ctx, Tag);
            else
                writtenChars = WriteString(ctx, " ");

            SpriteObj = new Sprites.Sprite(chunk.Data);
            SpriteOffset = ctx.RenderPosition + writtenChars;

            return new CommandResult(true, writtenChars);
        }

        private BinaryChunk GetChunk(EntryRenderContext ctx)
        {
            if (HasArgument("BI"))
            {
                var binaryIndex = GetArgument("BI");
                if (binaryIndex == null || !int.TryParse(binaryIndex, out var id) || ctx.Document.BinaryChunks == null)
                    return null;
                return ctx.Document.BinaryChunks.FirstOrDefault(bc => bc.Id == id);
            }
            
            if (HasArgument("A"))
            {
                var blob = Convert.FromBase64String(Aux);
                return new BinaryChunk(uint.MaxValue, 0, (uint)blob.Length, 0, blob);
            }

            return null;
        }

        public override string ToString() => AsString("SP");
    }
}
