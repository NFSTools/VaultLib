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

[VltTypeInfo(nameof(CCarDamageEntry))]
public class CCarDamageEntry : VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings
{
    public int PartID { get; set; }
    public string AttachPart { get; set; } = string.Empty;
    public RefSpec32 Material { get; set; } = new();
    public string SmackableCollisionName { get; set; } = string.Empty;
    public RefSpec32 SmackableCollisionAttribute { get; set; } = new();

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        PartID = br.ReadInt32();
        AttachPart = context.ReadString(br);
        Material.Read(context, fieldContext, br);
        SmackableCollisionName = context.ReadString(br);
        SmackableCollisionAttribute.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(PartID);
        context.WriteString(AttachPart, fieldContext, bw);
        Material.Write(context, fieldContext, bw);
        context.WriteString(SmackableCollisionName, fieldContext, bw);
        SmackableCollisionAttribute.Write(context, fieldContext, bw);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { AttachPart, SmackableCollisionName };
    }

    public override object Clone()
    {
        return new CCarDamageEntry
        {
            PartID = PartID,
            AttachPart = AttachPart,
            Material = (RefSpec32)Material.Clone(),
            SmackableCollisionName = SmackableCollisionName,
            SmackableCollisionAttribute = (RefSpec32)SmackableCollisionAttribute.Clone(),
        };
    }
}