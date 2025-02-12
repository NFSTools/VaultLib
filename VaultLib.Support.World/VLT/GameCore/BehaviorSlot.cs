// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:31 PM.

using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore;

[VltTypeInfo("GameCore::BehaviorSlot")]
public struct BehaviorSlot : IComplexType
{
    public enum BehaviorFlag
    {
        kBehavior_Activatable = 1,
        kBehavior_AutoActive
    }

    public Key32 mBehaviorChannel;
    public uint mBehaviorType;
    public BehaviorFlag mFlags;

    public void EndianSwap()
    {
        throw new System.NotImplementedException();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}