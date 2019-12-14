// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 7:19 PM.

using System;

namespace VaultLib.Core.Data
{
    [Flags]
    public enum DefinitionFlags : byte
    {
        Array = 0x1,
        InLayout = 0x2,
        IsBound = 0x4,
        IsNotSearchable = 0x8,
        IsStatic = 0x10,
        HasHandler = 0x20
    }
}