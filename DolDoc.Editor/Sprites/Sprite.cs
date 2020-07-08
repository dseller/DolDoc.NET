using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DolDoc.Editor.Sprites
{
    public class Sprite
    {
        private List<SpriteElementBase> _spriteElements;

        public Sprite(byte[] data)
        {
            _spriteElements = new List<SpriteElementBase>();
            Load(data);
        }

        private void Load(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(ms))
                {
                    while (true)
                    {
                        var elementType = (SpriteElementType)reader.ReadByte();
                        if (elementType == SpriteElementType.End)
                            break;

                        switch (elementType)
                        {
                            case SpriteElementType.Arrow:
                                _spriteElements.Add(new Arrow(reader));
                                break;

                            case SpriteElementType.Color:
                                _spriteElements.Add(new Color(reader));
                                break;

                            case SpriteElementType.Line:
                                _spriteElements.Add(new Line(reader));
                                break;

                            case SpriteElementType.Text:
                                _spriteElements.Add(new Text(reader));
                                break;

                            case SpriteElementType.TextBox:
                                _spriteElements.Add(new TextBox(reader));
                                break;

                            case SpriteElementType.Thick:
                                _spriteElements.Add(new Thick(reader));
                                break;

                            default:
                                Console.WriteLine("Encountered unsupported sprite element '{0}', aborting sprite loading", elementType);
                                return;     // Return here to avoid garbage being read.
                        }
                    }
                }
            }
        }

        public void WriteToFrameBuffer(byte[] frameBuffer, int pixelOffset)
        {
            var ctx = new SpriteRenderContext();
            foreach (var element in _spriteElements)
            {
                element.Render(ctx, frameBuffer, pixelOffset);
                Console.WriteLine("Rendered {0}", element);
            }
        }
    }
}
