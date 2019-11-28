// This file is part of VaultLib.Support.Carbon by heyitsleo.
// 
// Created: 11/03/2019 @ 11:27 AM.

using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(InputUpdateType))]
    public enum InputUpdateType
    {
        kUpdate = 0x0,
        kPress = 0x1,
        kRelease = 0x2,
        kAnalogPress = 0x3,
        kAnalogRelease = 0x4,
        kCenterControl = 0x5,
    }
}