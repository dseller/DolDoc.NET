using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public class Sprite : DocumentEntry
    {
        public Sprites.Sprite SpriteObj { get; private set; }

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

            SpriteObj = new Sprites.Sprite(chunk.Data);

            return new CommandResult(true);
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
