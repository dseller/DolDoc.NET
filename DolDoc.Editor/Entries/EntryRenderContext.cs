using DolDoc.Editor.Core;
using System.Collections.Generic;

namespace DolDoc.Editor.Commands
{
    public class EntryRenderContext
    {
        private readonly Stack<RenderOptions> renderOptionsStack;

        public EntryRenderContext(Document document, ViewerState state, RenderOptions renderOptions)
        {
            State = state;
            Document = document;
            RenderPosition = 0;
            renderOptionsStack = new Stack<RenderOptions>();
            renderOptionsStack.Push(renderOptions);
        }

        /// <summary>
        /// The current Document.
        /// </summary>
        public Document Document { get; }

        public ViewerState State { get; }

        /// <summary>
        /// The position where to render the entry.
        /// </summary>
        public int RenderPosition { get; set; }

        public RenderOptions Options => renderOptionsStack.Peek();

        public void PushOptions(RenderOptions renderOptions) => renderOptionsStack.Push(renderOptions);

        public void PopOptions() => renderOptionsStack.Pop();

        public RenderOptions NewOptions()
        {
            var options = Options.Clone();
            PushOptions(options);
            return options;
        }
    }
}
