using System;
using System.Collections.Generic;
using System.Linq;

// This piece table implementation was ported from jdormit's JavaScript piece table implementation: https://github.com/sparkeditor/piece-table/blob/master/index.js
// Note that I don't believe it is a perfect implementation of it, because the piece table should be append-only. This version mutates already existing entries in the piece table.

namespace DolDoc.Editor.Core
{
    public class Piece
    {
        public Piece(int start, int length, bool addBuffer)
        {
            Start = start;
            Length = length;
            AddBuffer = addBuffer;
        }

        public int Start { get; set; }

        public int Length { get; set; }

        public bool AddBuffer { get; private set; }

        public override string ToString()
        {
            return $"Start={Start}, Length={Length}, Add={AddBuffer}";
        }
    }

    public class PieceTable
    {
        public PieceTable(string original = null)
        {
            Add = string.Empty;
            Original = original ?? string.Empty;
            Pieces = new List<Piece>();

            Pieces.Add(new Piece(0, original?.Length ?? 0, false));
        }

        public string Original { get; private set; }

        public string Add { get; set; }

        public List<Piece> Pieces { get; private set; }

        public override string ToString()
        {
            string result = string.Empty;
            foreach (var piece in Pieces)
            {
                if (piece.AddBuffer)
                    result += Add.Substring(piece.Start, piece.Length);
                else
                    result += Original.Substring(piece.Start, piece.Length);
            }

            return result;
        }

        /// <summary>
        /// Inserts a string at the specified position.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="position"></param>
        public void Insert(string str, int position)
        {
            if (string.IsNullOrEmpty(str))
                return;

            int addBufferOffset = Add.Length;
            Add += str;

            var (pieceIndex, bufferOffset) = GetIndicesForStringOffset(position);
            var originalPiece = Pieces[pieceIndex];
            if (originalPiece.AddBuffer &&
                bufferOffset == originalPiece.Start + originalPiece.Length &&
                originalPiece.Start + originalPiece.Length == addBufferOffset)
            {
                originalPiece.Length += str.Length;
                return;
            }

            var insertPieces = new[]
            {
                new Piece(originalPiece.Start, bufferOffset - originalPiece.Start, originalPiece.AddBuffer),
                new Piece(addBufferOffset, str.Length, true),
                new Piece(bufferOffset, originalPiece.Length - (bufferOffset - originalPiece.Start), originalPiece.AddBuffer)
            }.Where(p => p.Length > 0);

            Pieces.RemoveAt(pieceIndex);
            Pieces.InsertRange(pieceIndex, insertPieces);
        }

        /// <summary>
        /// Removes a string from the specified span.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void Remove(int offset, int length)
        {
            if (length == 0)
                return;
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));

            var (initialPieceIndex, initialBufferOffset) = GetIndicesForStringOffset(offset);
            var (finalPieceIndex, finalBufferOffset) = GetIndicesForStringOffset(offset + length);

            if (initialPieceIndex == finalPieceIndex)
            {
                var piece = Pieces[initialPieceIndex];
                if (initialBufferOffset == piece.Start)
                {
                    piece.Start += length;
                    piece.Length -= length;
                    return;
                }
                else if (finalPieceIndex == piece.Start + piece.Length)
                {
                    piece.Length -= length;
                    return;
                }
            }

            var deletePieces = new[]
            {
                new Piece(Pieces[initialPieceIndex].Start, initialBufferOffset - Pieces[initialPieceIndex].Start, Pieces[initialPieceIndex].AddBuffer),
                new Piece(finalBufferOffset, Pieces[finalPieceIndex].Length - (finalBufferOffset - Pieces[finalPieceIndex].Start), Pieces[finalPieceIndex].AddBuffer)
            }.Where(p => p.Length > 0);

            Pieces.RemoveRange(initialPieceIndex, finalPieceIndex - initialPieceIndex + 1);
            Pieces.InsertRange(initialPieceIndex, deletePieces);
        }

        private (int, int) GetIndicesForStringOffset(int offset)
        {
            if (offset < 0)
                throw new ArgumentException("Offset can not be less than 0", nameof(offset));

            var remaining = offset;
            for (int i = 0; i < Pieces.Count; i++)
            {
                var piece = Pieces[i];
                if (remaining <= piece.Length)
                    return (i, piece.Start + remaining);
                remaining -= piece.Length;
            }

            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is out of range");
        }
    }
}
