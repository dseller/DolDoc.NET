using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DolDoc.Editor.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static string ReadNullTerminatedString(this BinaryReader reader)
        {
            var buffer = new List<byte>();

            while (reader.PeekChar() != 0x00 && reader.BaseStream.Position < reader.BaseStream.Length)
                buffer.Add(reader.ReadByte());

            if (reader.PeekChar() == 0x00)
                reader.ReadByte();

            return Encoding.ASCII.GetString(buffer.ToArray());
        }

        public static void WriteNullTerminatedString(this BinaryWriter writer, string str)
        {
            foreach (var ch in Encoding.ASCII.GetBytes(str))
                writer.Write(ch);
            writer.Write((byte)0);
        }
    }
}
