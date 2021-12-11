// <copyright file="Background.cs" company="Dennis Seller">
// Copyright (c) Dennis Seller. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using DolDoc.Editor.Commands;
using DolDoc.Editor.Core;

namespace DolDoc.Editor.Entries
{
    public class Background : DocumentEntry
    {
        public Background(IList<Flag> flags, IList<Argument> args)
            : base(flags, args)
        {
        }

        public override CommandResult Evaluate(EntryRenderContext ctx)
        {
            if (Arguments.Count == 0)
                ctx.Options.BackgroundColor = ctx.Options.DefaultBackgroundColor;
            else
                ctx.Options.BackgroundColor = (EgaColor)Enum.Parse(typeof(EgaColor), Tag, true);

            return new CommandResult(true);
        }

        public override string ToString() => AsString("BG");
    }
}
