// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 9:26 AM.

using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Commerce;

[VltTypeInfo("Commerce::HatBonus")]
public struct HatBonus : IComplexType
{
    public int Handling;
    public int Acceleration;
    public int TopSpeed;
    public int RequiredPartCount;

    public void EndianSwap()
    {
        throw new System.NotImplementedException();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}