// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 7:19 PM.

using System;

namespace VaultLib.Core.Data
{
    [Flags]
    public enum DefinitionFlags : byte
    {
        kArray = 0x1,
        kInLayout = 0x2,
        kIsBound = 0x4,
        kIsNotSearchable = 0x8,
        kIsStatic = 0x10,
        kHasHandler = 0x20
    }
}