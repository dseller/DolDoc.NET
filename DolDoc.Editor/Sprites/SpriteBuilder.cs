using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DolDoc.Editor.Sprites
{
    public class SpriteBuilder
    {
        private readonly List<SpriteElementBase> elements;

        public SpriteBuilder(params SpriteElementBase[] elements)
        {
            this.elements = elements.ToList();
        }

        public void Add(SpriteElementBase element) => elements.Add(element);

        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    foreach (var element in elements)
                        element.Serialize(writer);
                    writer.Write((byte)SpriteElementType.End);
                }

                return ms.ToArray();
            }
        }
    }
}
