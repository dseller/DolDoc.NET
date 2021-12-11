// <copyright file="RenderOptions.cs" company="Dennis Seller">
// Copyright (c) Dennis Seller. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace DolDoc.Editor.Core
{
    public class RenderOptions
    {
        public RenderOptions(EgaColor defaultFgColor = EgaColor.Black, EgaColor defaultBgColor = EgaColor.White)
        {
            DefaultBackgroundColor = defaultBgColor;
            DefaultForegroundColor = defaultFgColor;
            BackgroundColor = defaultBgColor;
            ForegroundColor = defaultFgColor;
        }

        public EgaColor ForegroundColor { get; set; }

        public EgaColor BackgroundColor { get; set; }

        public EgaColor DefaultForegroundColor { get; set; }

        public EgaColor DefaultBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether underline mode is enabled.
        /// </summary>
        public bool Underline { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether blinking mode is enabled.
        /// </summary>
        public bool Blink { get; set; }

        public bool WordWrap { get; set; }

        public bool Inverted { get; set; }

        /// <summary>
        /// Gets or sets the indentation level.
        /// </summary>
        public int Indentation { get; set; }

        public int? CollapsedTreeNodeIndentationLevel { get; set; }

        public RenderOptions Clone() => new RenderOptions(DefaultForegroundColor, DefaultBackgroundColor)
        {
            Underline = Underline,
            Blink = Blink,
            WordWrap = WordWrap,
            Inverted = Inverted,
            Indentation = Indentation,
            CollapsedTreeNodeIndentationLevel = CollapsedTreeNodeIndentationLevel,
        };
    }
}
