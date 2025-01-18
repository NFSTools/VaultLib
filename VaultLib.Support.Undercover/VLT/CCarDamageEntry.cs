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

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            PartID = br.ReadInt32();
            _attachPartText.Read(context, br);
            Material.Read(context, br);
            _smackableCollisionNameText.Read(context, br);
            SmackableCollisionAttribute.Read(context, br);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            bw.Write(PartID);
            _attachPartText.Value = AttachPart;
            _attachPartText.Write(context, bw);
            Material.Write(context, bw);
            _smackableCollisionNameText.Value = SmackableCollisionName;
            _smackableCollisionNameText.Write(context, bw);
            SmackableCollisionAttribute.Write(context, bw);
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            _attachPartText.ReadPointerData(context, br);
            _smackableCollisionNameText.ReadPointerData(context, br);
            AttachPart = _attachPartText.Value;
            SmackableCollisionName = _smackableCollisionNameText.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _attachPartText.WritePointerData(context, bw);
            _smackableCollisionNameText.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            _attachPartText.AddPointers(context);
            _smackableCollisionNameText.AddPointers(context);
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