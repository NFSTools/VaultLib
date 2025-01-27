// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:35 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(CameraCurveReactionRecord))]
public class CameraCurveReactionRecord: VltBaseType<uint>, IVltPointerObject<uint>
{
    public Curve Curve { get; set; } = new();

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        if (br.ReadUInt32() != 0)
            throw new InvalidDataException();
        Curve.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(0);
        Curve.Write(context, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Curve.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        Curve.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext)
    {
        Curve.AddPointers(context, fieldContext);
    }
}