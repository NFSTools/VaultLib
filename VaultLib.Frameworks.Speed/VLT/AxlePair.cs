// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 3:54 PM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(AxlePair))]
public struct AxlePair
{
    public float Front;
    public float Rear;

    public override string ToString()
    {
        return $"[{Front}, {Rear}]";
    }
}