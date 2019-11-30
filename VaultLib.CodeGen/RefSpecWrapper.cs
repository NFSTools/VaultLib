// This file is part of VaultLib.CodeGen by heyitsleo.
// 
// Created: 11/29/2019 @ 10:56 PM.

using System;
using VaultLib.Core.Data;

namespace VaultLib.CodeGen
{
    public class RefSpecWrapper
    {
        public RefSpecWrapper(VLTClass @class, VLTCollection collection)
        {
            Class = @class;
            Collection = collection;
        }

        public VLTClass Class { get; }
        public VLTCollection Collection { get; }

        public TW Get<TW>() where TW : CollectionWrapperBase, new()
        {
            return (TW) Activator.CreateInstance(typeof(TW), Collection);
        }
    }
}