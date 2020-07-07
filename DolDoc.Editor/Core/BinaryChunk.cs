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

        public uint Id { get; }

        public uint Flags { get; }

        public uint Size { get; }

        public uint RefCount { get; }

        public byte[] Data { get; }
    }
}
