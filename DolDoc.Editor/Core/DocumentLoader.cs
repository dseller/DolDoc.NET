// <copyright file="DocumentLoader.cs" company="Dennis Seller">
// Copyright (c) Dennis Seller. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.IO;
using DolDoc.Editor.Extensions;
using Serilog;

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

                    Log.Information("Read binary chunk ID {0} with size {1}", chunkId, size);

                    binaryChunks.Add(new BinaryChunk(chunkId, flags, size, refCount, binaryData));
                }

                return new Document(text.Replace("\r\n", "\n"), columns, rows, binaryChunks: binaryChunks);
            }
        }
    }
}
