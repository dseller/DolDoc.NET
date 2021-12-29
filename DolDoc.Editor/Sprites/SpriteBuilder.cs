using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolDoc.Editor.Sprites
{
    public class SpriteBuilder
    {
        private readonly List<SpriteElementBase> elements;

        public SpriteBuilder()
        {
            elements = new List<SpriteElementBase>();
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
