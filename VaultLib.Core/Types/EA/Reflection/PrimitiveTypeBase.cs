// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/04/2019 @ 7:07 PM.

using System;

namespace VaultLib.Core.Types.EA.Reflection
{
    public abstract class PrimitiveTypeBase : VLTBaseType
    {
        public abstract IConvertible GetValue();
        public abstract void SetValue(IConvertible value);
    }
}