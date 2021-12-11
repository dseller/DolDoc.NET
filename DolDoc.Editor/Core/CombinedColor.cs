// <copyright file="CombinedColor.cs" company="Dennis Seller">
// Copyright (c) Dennis Seller. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace DolDoc.Editor.Core
{
    public class CombinedColor
    {
        public CombinedColor(byte color)
        {
            Value = color;
        }

        public CombinedColor(EgaColor background, EgaColor foreground)
        {
            Value = (byte)((((byte)foreground & 0x0F) << 4) | ((byte)background & 0x0F));
        }

        public EgaColor Background
        {
            get => (EgaColor)(Value & 0x0F);
            set => Value = (byte)((byte)(Value & 0xF0) | ((byte)value & 0x0F));
        }

        public EgaColor Foreground
        {
            get => (EgaColor)((Value >> 4) & 0x0F);
            set => Value = (byte)((byte)(Value & 0x0F) | (((byte)value & 0x0F) << 4));
        }

        public byte Value { get; set; }
    }
}
