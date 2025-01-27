// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:38 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.MostWanted.VLT;

[VltTypeInfo(nameof(JunkmanMod))]
public class JunkmanMod: VltBaseType<uint>
{
    public uint ClassKey { get; set; }
    public uint DefinitionKey { get; set; }
    public float ScaleF { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        ClassKey = br.ReadUInt32();
        DefinitionKey = br.ReadUInt32();
        ScaleF = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(ClassKey);
        bw.Write(DefinitionKey);
        bw.Write(ScaleF);
    }
}