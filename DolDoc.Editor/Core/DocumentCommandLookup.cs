// <copyright file="DocumentCommandLookup.cs" company="Dennis Seller">
// Copyright (c) Dennis Seller. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;

namespace DolDoc.Editor.Core
{
    public static class DocumentCommandLookup
    {
        private static readonly IDictionary<string, DocumentCommand> Values =
            new Dictionary<string, DocumentCommand>()
            {
                { "BG", DocumentCommand.Background },
                { "BK", DocumentCommand.Blink },
                { "CL", DocumentCommand.Clear },
                { "CM", DocumentCommand.MoveCursor },
                { "FG", DocumentCommand.Foreground },
                { "ID", DocumentCommand.Indent },
                { "IV", DocumentCommand.Invert },
                { "LK", DocumentCommand.Link },
                { "TR", DocumentCommand.Tree },
                { "TX", DocumentCommand.Text },
                { "UL", DocumentCommand.Underline },
                { "WW", DocumentCommand.WordWrap },
            };

        public static DocumentCommand Get(string cmd)
        {
            if (!Values.TryGetValue(cmd, out var command))
                return DocumentCommand.Error;

            return command;
        }
    }
}
