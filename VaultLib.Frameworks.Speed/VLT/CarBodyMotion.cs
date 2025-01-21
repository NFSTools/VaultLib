// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 9:15 PM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CarBodyMotion))]
public struct CarBodyMotion
{
    public float DegPerG;
    public float MaxGs;
    public float DegPerSec;
}