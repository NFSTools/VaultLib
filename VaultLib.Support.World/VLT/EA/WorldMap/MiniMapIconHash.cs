// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:28 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.EA.WorldMap;

[VltTypeInfo("EA::WorldMap::MiniMapIconHash")]
public class MiniMapIconHash: VltBaseType<Core.DataInterfaces.Key32>
{
    public uint Hash { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Hash = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Hash);
    }
}