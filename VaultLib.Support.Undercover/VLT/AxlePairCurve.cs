// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:19 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(AxlePairCurve))]
public class AxlePairCurve : VltBaseType, IVltPointerObject
{
    public Curve Front { get; set; } = new();
    public Curve Rear { get; set; } = new();

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        Front.Read(context, fieldContext, br);
        Rear.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        Front.Write(context, fieldContext, bw);
        Rear.Write(context, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        Front.ReadPointerData(context, fieldContext, br);
        Rear.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        Front.WritePointerData(context, fieldContext, bw);
        Rear.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
    {
        Front.AddPointers(context, fieldContext);
        Rear.AddPointers(context, fieldContext);
    }
}