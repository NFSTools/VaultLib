// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/15/2019 @ 4:31 PM.

using System;
using VaultLib.Core.Data;

namespace VaultLib.Core.Exceptions
{
    public class CollectionLoadingException : Exception
    {
        public VLTCollection Collection { get; }

        public CollectionLoadingException(string message, VLTCollection collection) : base(message)
        {
            this.Collection = collection ?? throw new ArgumentNullException(nameof(collection)); ;
        }

        public CollectionLoadingException(string message, Exception innerException, VLTCollection collection) : base(message, innerException)
        {
            this.Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }
    }
}