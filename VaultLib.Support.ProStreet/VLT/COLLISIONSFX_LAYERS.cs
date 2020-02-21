// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 3:26 PM.

using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(COLLISIONSFX_LAYERS))]
    public enum COLLISIONSFX_LAYERS : uint
    {
        LAYER_NONE = 0xFFFFFFFF,
        LAYER_CORE = 0x1,
        LAYER_ZONE = 0x2,
        LAYER_SURFACE = 0x3,
        LAYER_OBJECT = 0x4,
        LAYER_SWEETENERS = 0x5,
    }
}