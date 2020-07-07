using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DolDoc.Editor.Core
{
    public static class DocumentLoader
    {
        public static Document Load(Stream stream)
        {
            var binaryChunks = new List<BinaryChunk>();

            using (var reader = new BinaryReader(stream))
            {
                string text = ReadTextSection(reader);

                if (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    // Discard the zero
                    if (reader.PeekChar() == 0x00)
                        reader.ReadByte();

                    uint chunkId = reader.ReadUInt32();
                    uint flags = reader.ReadUInt32();
                    uint size = reader.ReadUInt32();
                    uint refCount = reader.ReadUInt32();
                    byte[] binaryData = reader.ReadBytes((int)size);

                    Console.WriteLine("Read binary chunk ID {0} with size {1}", chunkId, size);

                    binaryChunks.Add(new BinaryChunk(chunkId, flags, size, refCount, binaryData));
                }

                return new Document(text.Replace("\r\n", "\n"), binaryChunks: binaryChunks);
            }
        }

        private static string ReadTextSection(BinaryReader reader)
        {
            int pos = 0;
            byte[] buffer = new byte[reader.BaseStream.Length];

            while (reader.PeekChar() != 0x00 && reader.BaseStream.Position < reader.BaseStream.Length)
                buffer[pos++] = reader.ReadByte();

            return Encoding.ASCII.GetString(buffer);
        }
    }
}
