// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:35 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(CameraCurveReactionRecord))]
public class CameraCurveReactionRecord : VltBaseType<Core.DataInterfaces.Key32>,
    IVltPointerObject<Core.DataInterfaces.Key32>
{
    public Curve Curve { get; set; } = new();

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        if (br.ReadUInt32() != 0)
            throw new InvalidDataException();
        Curve.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(0);
        Curve.Write(context, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Curve.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        Curve.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext)
    {
        Curve.AddPointers(context, fieldContext);
    }

    public override object Clone()
    {
        return new CameraCurveReactionRecord
        {
            Curve = (Curve)Curve.Clone(),
        };
    }
}