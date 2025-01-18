// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 10:55 PM.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VltTypeInfo(nameof(CCarDamageEntry))]
    public class CCarDamageEntry : VltBaseType, IReferencesStrings
    {
        public int PartID { get; set; }
        public string AttachPart { get; set; } = string.Empty;
        public RefSpec Material { get; set; } = new();
        public string SmackableCollisionName { get; set; } = string.Empty;
        public RefSpec SmackableCollisionAttribute { get; set; } = new();

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            PartID = br.ReadInt32();
            AttachPart = context.ReadString(br);
            Material.Read(context, fieldContext, br);
            SmackableCollisionName = context.ReadString(br);
            SmackableCollisionAttribute.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(PartID);
            context.WriteString(AttachPart, fieldContext, bw);
            Material.Write(context, fieldContext, bw);
            context.WriteString(SmackableCollisionName, fieldContext, bw);
            SmackableCollisionAttribute.Write(context, fieldContext, bw);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { AttachPart, SmackableCollisionName };
        }
    }
}