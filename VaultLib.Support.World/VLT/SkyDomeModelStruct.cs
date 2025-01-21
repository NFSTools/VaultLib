// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 7:23 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT;

[VltTypeInfo(nameof(SkyDomeModelStruct))]
public class SkyDomeModelStruct : VltBaseType
{
    public uint Hash { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        Hash = br.ReadUInt32(); // SKYDOME_1_DAWN, SKYDOME_1_DUSK, etc
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        bw.Write(Hash);
    }
}