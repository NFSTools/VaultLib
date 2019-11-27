// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:38 PM.

using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore.Collision
{
    [VLTTypeInfo("GameCore::Collision::TriggerType")]
    public enum TriggerType
    {
        Unknown,
        Cylinder,
        Box,
        Plane
    }
}