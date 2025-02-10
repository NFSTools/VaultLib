// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:38 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(JunkmanMod))]
public class JunkmanMod : VltBaseType<Key32>
{
    public Key32 ClassKey { get; set; }
    public Key32 DefinitionKey { get; set; }
    public float ScaleF { get; set; }
    public float ScaleR { get; set; }

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        ClassKey = Key32.Read(br);
        DefinitionKey = Key32.Read(br);
        ScaleF = br.ReadSingle();
        ScaleR = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        ClassKey.Write(bw);
        DefinitionKey.Write(bw);
        bw.Write(ScaleF);
        bw.Write(ScaleR);
    }

    public override object Clone()
    {
        return new JunkmanMod
        {
            ClassKey = this.ClassKey,
            DefinitionKey = this.DefinitionKey,
            ScaleF = this.ScaleF,
            ScaleR = this.ScaleR,
        };
    }
}