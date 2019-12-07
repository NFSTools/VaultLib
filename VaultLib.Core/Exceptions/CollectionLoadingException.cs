// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/15/2019 @ 4:31 PM.

using System;
using VaultLib.Core.Data;

namespace VaultLib.Core.Exceptions
{
    public class CollectionLoadingException : Exception
    {
        public CollectionLoadingException(string message, VLTCollection collection) : base(message)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public CollectionLoadingException(string message, Exception innerException, VLTCollection collection) : base(
            message, innerException)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public VLTCollection Collection { get; }
    }
}