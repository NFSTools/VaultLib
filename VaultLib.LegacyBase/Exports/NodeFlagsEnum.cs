using System;

namespace VaultLib.LegacyBase.Exports
{
    [Flags]
    public enum NodeFlagsEnum : ushort
    {
        Default = 0x00,
        IsArray = 0x02,
        IsInline = 0x20
    }
}