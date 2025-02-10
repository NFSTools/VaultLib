// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:12 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.AI;

[VltTypeInfo("AI::GlueCurve")]
public class GlueCurve : VltBaseType<Core.DataInterfaces.Key32>, IVltPointerObject<Core.DataInterfaces.Key32>
{
    public Curve Easy { get; set; } = new();
    public Curve Hard { get; set; } = new();

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Easy.Read(context, fieldContext, br);
        Hard.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        Easy.Write(context, fieldContext, bw);
        Hard.Write(context, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Easy.ReadPointerData(context, fieldContext, br);
        Hard.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        Easy.WritePointerData(context, fieldContext, bw);
        Hard.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext)
    {
        Easy.AddPointers(context, fieldContext);
        Hard.AddPointers(context, fieldContext);
    }

    public override object Clone()
    {
        return new GlueCurve
        {
            Easy = (Curve)Easy.Clone(),
            Hard = (Curve)Hard.Clone(),
        };
    }
}