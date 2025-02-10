// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 3:54 PM.

using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(AxlePair))]
public struct AxlePair : IComplexType
{
    public float Front;
    public float Rear;

    public override string ToString()
    {
        return $"[{Front}, {Rear}]";
    }

    public void EndianSwap()
    {
        Front = Front.EndianSwap();
        Rear = Rear.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}