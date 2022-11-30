using DolDoc.Editor.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DolDoc.Editor.Core
{
    public static class DocumentLoader
    {
        public static Document Load(Stream stream, string path)
        {
            var binaryChunks = new List<BinaryChunk>();

            using (var reader = new BinaryReader(stream))
            {
                string text = reader.ReadNullTerminatedString();

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    uint chunkId = reader.ReadUInt32();
                    uint flags = reader.ReadUInt32();
                    uint size = reader.ReadUInt32();
                    uint refCount = reader.ReadUInt32();
                    byte[] binaryData = reader.ReadBytes((int)size);

                    Debug.WriteLine($"Read binary chunk ID {chunkId} with size {size}");

                    binaryChunks.Add(new BinaryChunk(chunkId, flags, size, refCount, binaryData));
                }

                return new Document(text.Replace("\r\n", "\n"), binaryChunks: binaryChunks, path);
            }
        }
    }
}
