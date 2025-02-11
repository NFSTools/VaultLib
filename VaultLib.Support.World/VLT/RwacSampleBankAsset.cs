// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 3:43 PM.

using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT;

[VltTypeInfo(nameof(RwacSampleBankAsset))]
public struct RwacSampleBankAsset : IComplexType
{
    public uint Bank;
    public uint Asset;

    public override string ToString()
    {
        return $"RWAC Bank {Bank:X8} -> Asset {Asset:X8}";
    }

    public void EndianSwap()
    {
        throw new System.NotImplementedException();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}