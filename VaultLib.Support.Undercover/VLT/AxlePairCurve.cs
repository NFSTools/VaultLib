// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:19 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(AxlePairCurve))]
public class AxlePairCurve : VltBaseType<Core.DataInterfaces.Key32>, IVltPointerObject<Core.DataInterfaces.Key32>
{
    public Curve Front { get; set; } = new();
    public Curve Rear { get; set; } = new();

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Front.Read(context, fieldContext, br);
        Rear.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        Front.Write(context, fieldContext, bw);
        Rear.Write(context, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Front.ReadPointerData(context, fieldContext, br);
        Rear.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        Front.WritePointerData(context, fieldContext, bw);
        Rear.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext)
    {
        Front.AddPointers(context, fieldContext);
        Rear.AddPointers(context, fieldContext);
    }

    public override object Clone()
    {
        return new AxlePairCurve
        {
            Front = (Curve)Front.Clone(),
            Rear = (Curve)Rear.Clone(),
        };
    }
}