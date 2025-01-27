// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:19 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(AxlePairCurve))]
public class AxlePairCurve: VltBaseType<uint>, IVltPointerObject<uint>
{
    public Curve Front { get; set; } = new();
    public Curve Rear { get; set; } = new();

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Front.Read(context, fieldContext, br);
        Rear.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        Front.Write(context, fieldContext, bw);
        Rear.Write(context, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Front.ReadPointerData(context, fieldContext, br);
        Rear.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        Front.WritePointerData(context, fieldContext, bw);
        Rear.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext)
    {
        Front.AddPointers(context, fieldContext);
        Rear.AddPointers(context, fieldContext);
    }
}