namespace DolDoc.Editor.Core
{
    public class BinaryChunk
    {
        public BinaryChunk(uint id, uint flags, uint size, uint refCount, byte[] data)
        {
            Id = id;
            Flags = flags;
            Size = size;
            RefCount = refCount;
            Data = data;
        }

        uint Id { get; }

        uint Flags { get; }

        uint Size { get; }

        uint RefCount { get; }

        byte[] Data { get; }
    }
}
