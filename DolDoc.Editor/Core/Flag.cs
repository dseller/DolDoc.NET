// <copyright file="Flag.cs" company="Dennis Seller">
// Copyright (c) Dennis Seller. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace DolDoc.Editor.Core
{
    public class Flag
    {
        public Flag(bool status, string value)
        {
            Status = status;
            Value = value;
        }

        public bool Status { get; }

        public string Value { get; }
    }
}
