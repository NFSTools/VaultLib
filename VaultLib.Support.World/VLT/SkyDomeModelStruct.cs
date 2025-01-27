// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 7:23 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT;

[VltTypeInfo(nameof(SkyDomeModelStruct))]
public class SkyDomeModelStruct: VltBaseType<uint>
{
    public uint Hash { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Hash = br.ReadUInt32(); // SKYDOME_1_DAWN, SKYDOME_1_DUSK, etc
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(Hash);
    }
}