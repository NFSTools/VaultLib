// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 10:55 PM.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
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
        public string AttachPart { get; set; }
        public RefSpec Material { get; set; }
        public string SmackableCollisionName { get; set; }
        public RefSpec SmackableCollisionAttribute { get; set; }

        private Text _attachPartText;
        private Text _smackableCollisionNameText;

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            PartID = br.ReadInt32();
            _attachPartText.Read(context, fieldContext, br);
            Material.Read(context, fieldContext, br);
            _smackableCollisionNameText.Read(context, fieldContext, br);
            SmackableCollisionAttribute.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(PartID);
            _attachPartText.Value = AttachPart;
            _attachPartText.Write(context, fieldContext, bw);
            Material.Write(context, fieldContext, bw);
            _smackableCollisionNameText.Value = SmackableCollisionName;
            _smackableCollisionNameText.Write(context, fieldContext, bw);
            SmackableCollisionAttribute.Write(context, fieldContext, bw);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _attachPartText.ReadPointerData(context, fieldContext, br);
            _smackableCollisionNameText.ReadPointerData(context, fieldContext, br);
            AttachPart = _attachPartText.Value;
            SmackableCollisionName = _smackableCollisionNameText.Value;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _attachPartText.WritePointerData(context, fieldContext, bw);
            _smackableCollisionNameText.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            _attachPartText.AddPointers(context, fieldContext);
            _smackableCollisionNameText.AddPointers(context, fieldContext);
        }

        public IEnumerable<string> GetStrings()
        {
            return _attachPartText.GetStrings()
                .Concat(_smackableCollisionNameText.GetStrings());
        }

        public CCarDamageEntry(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _attachPartText = new Text(Class, Field, Collection);
            AttachPart = string.Empty;
            Material = new RefSpec(Class, Field, Collection);
            _smackableCollisionNameText = new Text(Class, Field, Collection);
            SmackableCollisionName = string.Empty;
            SmackableCollisionAttribute = new RefSpec(Class, Field, Collection);
        }
    }
}