using DolDoc.Editor.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DolDoc.Editor.Core
{
    public static class DocumentLoader
    {
        public static Document Load(Stream stream, int columns = 80, int rows = 60)
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

                    Console.WriteLine("Read binary chunk ID {0} with size {1}", chunkId, size);

                    binaryChunks.Add(new BinaryChunk(chunkId, flags, size, refCount, binaryData));
                }

                return new Document(text.Replace("\r\n", "\n"), columns, rows, binaryChunks: binaryChunks);
            }
        }
    }
}
