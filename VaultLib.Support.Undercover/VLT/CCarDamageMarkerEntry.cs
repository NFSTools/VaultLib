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
public class CCarDamageMarkerEntry : VltBaseType<Core.DataInterfaces.Key32>,
    IReferencesStrings
{
    public string MarkerName { get; set; } = string.Empty;
    public int PartID { get; set; }
    public int SlotID { get; set; }
    public string AttachPart { get; set; } = string.Empty;
    public string SmackableCollisionName { get; set; } = string.Empty;
    public RefSpec32 SmackableCollisionAttribute { get; set; } = new();

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        MarkerName = context.ReadString(br);
        PartID = br.ReadInt32();
        SlotID = br.ReadInt32();
        AttachPart = context.ReadString(br);
        SmackableCollisionName = context.ReadString(br);
        SmackableCollisionAttribute.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        context.WriteString(MarkerName, fieldContext, bw);
        bw.Write(PartID);
        bw.Write(SlotID);
        context.WriteString(AttachPart, fieldContext, bw);
        context.WriteString(SmackableCollisionName, fieldContext, bw);
        SmackableCollisionAttribute.Write(context, fieldContext, bw);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { MarkerName, AttachPart, SmackableCollisionName };
    }

    public override object Clone()
    {
        return new CCarDamageMarkerEntry
        {
            MarkerName = MarkerName,
            PartID = PartID,
            SlotID = SlotID,
            AttachPart = AttachPart,
            SmackableCollisionName = SmackableCollisionName,
            SmackableCollisionAttribute = (RefSpec32)SmackableCollisionAttribute.Clone()
        };
    }
}