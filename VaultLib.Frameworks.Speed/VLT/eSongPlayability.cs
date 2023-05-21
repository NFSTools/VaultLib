// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/09/2019 @ 9:47 AM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(eSongPlayability))]
    public enum eSongPlayability
    {
        ePLAY_OFF = 0x0,
        ePLAY_MENU = 0x1,
        ePLAY_RACE = 0x2,
        ePLAY_ALL = 0x3,
    }
}