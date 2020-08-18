using System;
using System.IO;

namespace DolDoc.Editor.Sprites
{
    public class Arrow : Line
    {
        public Arrow(BinaryReader reader) 
            : base(reader)
        {
        }

        public override void Render(SpriteRenderContext ctx, byte[] frameBuffer, int pixelOffset)
        {
            // First, render the line.
            base.Render(ctx, frameBuffer, pixelOffset);

            // Then, render the arrow head on top.
            RenderArrowHead(frameBuffer, pixelOffset, X1, Y1, X2, Y2);
        }

        /// <summary>
        /// Renders an arrow head at the end of the line.
        /// </summary>
        /// <param name="ctx">Render ctx</param>
        /// <param name="frameBuffer">Framebuffer</param>
        /// <param name="pixelOffset">Offset in framebuffer</param>
        /// <param name="x1">Source X</param>
        /// <param name="y1">Source Y</param>
        /// <param name="x2">Dest X</param>
        /// <param name="y2">Dest Y</param>
        /// <param name="size">Length in pixels of the arrow heads</param>
        protected void RenderArrowHead(byte[] frameBuffer, int pixelOffset, int x1, int y1, int x2, int y2, int size = 6)
        {
            var angle = Math.Atan2(y2 - y1, x2 - x1);

            RenderLine(frameBuffer, pixelOffset, x2, y2, (int)(x2 - size * Math.Cos(angle - Math.PI / 6)), (int)(y2 - size * Math.Sin(angle - Math.PI / 6)));
            RenderLine(frameBuffer, pixelOffset, x2, y2, (int)(x2 - size * Math.Cos(angle + Math.PI / 6)), (int)(y2 - size * Math.Sin(angle + Math.PI / 6)));
        }

        public override string ToString() => $"Arrow ({X1}, {Y1}) => ({X2}, {Y2})";
    }
}
