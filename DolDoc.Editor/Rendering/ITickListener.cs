﻿namespace DolDoc.Editor.Rendering
{
    public interface ITickListener
    {
        void Tick(ulong ticks, bool hasFocus);
    }
}
