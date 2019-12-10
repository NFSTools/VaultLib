using System;

namespace VaultLib.ModernBase.Exports
{
    [Flags]
    public enum NodeFlagsEnum : byte
    {
        Default = 0x00,
        IsArray = 0x02,
        HasHandler = 0x08,
        IsInline = 0x40
    }
}