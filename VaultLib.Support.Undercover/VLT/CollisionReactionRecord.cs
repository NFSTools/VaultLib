// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:27 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(CollisionReactionRecord))]
public class CollisionReactionRecord: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public float Elasticity { get; set; }
    public float RollHeight { get; set; }
    public float WeightBias { get; set; }
    public float MassScale { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Elasticity = br.ReadSingle();
        RollHeight = br.ReadSingle();
        WeightBias = br.ReadSingle();
        MassScale = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Elasticity);
        bw.Write(RollHeight);
        bw.Write(WeightBias);
        bw.Write(MassScale);
    }
}