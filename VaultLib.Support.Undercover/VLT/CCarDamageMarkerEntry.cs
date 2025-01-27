// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 10:55 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(CCarDamageMarkerEntry))]
public class CCarDamageMarkerEntry: VltBaseType<uint>, IReferencesStrings<uint>
{
    public string MarkerName { get; set; } = string.Empty;
    public int PartID { get; set; }
    public int SlotID { get; set; }
    public string AttachPart { get; set; } = string.Empty;
    public string SmackableCollisionName { get; set; } = string.Empty;
    public RefSpec<uint> SmackableCollisionAttribute { get; set; } = new();

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        MarkerName = context.ReadString(br);
        PartID = br.ReadInt32();
        SlotID = br.ReadInt32();
        AttachPart = context.ReadString(br);
        SmackableCollisionName = context.ReadString(br);
        SmackableCollisionAttribute.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        context.WriteString(MarkerName, fieldContext, bw);
        bw.Write(PartID);
        bw.Write(SlotID);
        context.WriteString(AttachPart, fieldContext, bw);
        context.WriteString(SmackableCollisionName, fieldContext, bw);
        SmackableCollisionAttribute.Write(context, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
    }

    public void WritePointerData(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
    }

    public void AddPointers(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext)
    {
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { MarkerName, AttachPart, SmackableCollisionName };
    }
}