// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:12 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.AI;

[VltTypeInfo("AI::GlueCurve")]
public class GlueCurve: VltBaseType<VaultLib.Core.DataInterfaces.Key32>, IVltPointerObject<VaultLib.Core.DataInterfaces.Key32>
{
    public Curve Easy { get; set; } = new();
    public Curve Hard { get; set; } = new();

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Easy.Read(context, fieldContext, br);
        Hard.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        Easy.Write(context, fieldContext, bw);
        Hard.Write(context, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Easy.ReadPointerData(context, fieldContext, br);
        Hard.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        Easy.WritePointerData(context, fieldContext, bw);
        Hard.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext)
    {
        Easy.AddPointers(context, fieldContext);
        Hard.AddPointers(context, fieldContext);
    }
}