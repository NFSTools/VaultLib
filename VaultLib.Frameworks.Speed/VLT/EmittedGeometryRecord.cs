// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 10:42 AM.

using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(EmittedGeometryRecord))]
public struct EmittedGeometryRecord
{
    public BinKey32 mEnum;
    public uint mIndex;
}