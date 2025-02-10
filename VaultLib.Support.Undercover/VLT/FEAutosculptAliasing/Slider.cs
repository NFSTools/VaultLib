// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:02 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.FEAutosculptAliasing;

public class Slider: VltBaseType<Core.DataInterfaces.Key32>
{
    public uint Region { get; set; }
    public uint Zone { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Region = br.ReadUInt32();
        Zone = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Region);
        bw.Write(Zone);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}