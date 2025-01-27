// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:38 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.MostWanted.VLT;

[VltTypeInfo(nameof(JunkmanMod))]
public class JunkmanMod: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public uint ClassKey { get; set; }
    public uint DefinitionKey { get; set; }
    public float ScaleF { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        ClassKey = br.ReadUInt32();
        DefinitionKey = br.ReadUInt32();
        ScaleF = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(ClassKey);
        bw.Write(DefinitionKey);
        bw.Write(ScaleF);
    }
}