// <copyright file="CommandResult.cs" company="Dennis Seller">
// Copyright (c) Dennis Seller. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace DolDoc.Editor.Core
{
    public class CommandResult
    {
        public CommandResult(bool success, int writtenCharacters = 0, bool refreshRequested = false)
        {
            Success = success;
            RefreshRequested = refreshRequested;
            WrittenCharacters = writtenCharacters;
        }

        public bool Success { get; }

        public int WrittenCharacters { get; }

        public bool RefreshRequested { get; }
    }
}
