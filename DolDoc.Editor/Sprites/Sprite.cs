using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace DolDoc.Editor.Sprites
{
    public class Sprite
    {
        private readonly List<SpriteElementBase> spriteElements;

        public Sprite(byte[] data)
        {
            spriteElements = new List<SpriteElementBase>();
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
                                spriteElements.Add(new Arrow(reader));
                                break;

                            case SpriteElementType.Bitmap:
                                spriteElements.Add(new Bitmap(reader));
                                break;

                            case SpriteElementType.Color:
                                spriteElements.Add(new Color(reader));
                                break;

                            case SpriteElementType.Line:
                                spriteElements.Add(new Line(reader));
                                break;

                            case SpriteElementType.Text:
                                spriteElements.Add(new Text(reader));
                                break;

                            case SpriteElementType.TextBox:
                                spriteElements.Add(new TextBox(reader));
                                break;

                            case SpriteElementType.Thick:
                                spriteElements.Add(new Thick(reader));
                                break;
                            
                            case SpriteElementType.Circle:
                                spriteElements.Add(new Circle(reader));
                                break;

                            default:
                                Console.WriteLine("Encountered unsupported sprite element '{0}', aborting sprite loading", elementType);
                                return;     // Return here to avoid garbage being read.
                        }
                    }
                }
            }
        }

        public void Render(ViewerState state, int x, int y)
        {
            GL.PushAttrib(AttribMask.ColorBufferBit);
            GL.Color3(0, 0, 0);
            var ctx = new SpriteRenderContext(state);
            foreach (var element in spriteElements)
                element.Render(ctx, x, y);
            GL.PopAttrib();
        }
    }
}
