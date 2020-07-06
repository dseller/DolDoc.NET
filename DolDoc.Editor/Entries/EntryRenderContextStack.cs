using DolDoc.Editor.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolDoc.Editor.Entries
{
    public class EntryRenderContextStack
    {
        private Stack<EntryRenderContext> _stack;

        public EntryRenderContextStack(EntryRenderContext ctx)
        {
            Context = ctx;
            _stack = new Stack<EntryRenderContext>();
        }

        public EntryRenderContext Context { get; private set; }

        public void Push() => _stack.Push((EntryRenderContext)Context.Clone());

        public void Pop() => Context = _stack.Pop();
    }
}
