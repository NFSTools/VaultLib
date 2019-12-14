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
    [VLTTypeInfo(nameof(CCarDamageMarkerEntry))]
    public class CCarDamageMarkerEntry : VLTBaseType, IReferencesStrings
    {
        public string MarkerName { get; set; }
        public int PartID { get; set; }
        public int SlotID { get; set; }
        public string AttachPart { get; set; }
        public string SmackableCollisionName { get; set; }
        public RefSpec SmackableCollisionAttribute { get; set; }

        private Text _markerNameText;
        private Text _attachPartText;
        private Text _smackableCollisionNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            _markerNameText = new Text(Class, Field, Collection);
            _markerNameText.Read(vault, br);
            PartID = br.ReadInt32();
            SlotID = br.ReadInt32();
            _attachPartText = new Text(Class, Field, Collection);
            _attachPartText.Read(vault, br);
            _smackableCollisionNameText = new Text(Class, Field, Collection);
            _smackableCollisionNameText.Read(vault, br);
            SmackableCollisionAttribute = new RefSpec(Class, Field, Collection);
            SmackableCollisionAttribute.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _markerNameText.Write(vault, bw);
            bw.Write(PartID);
            bw.Write(SlotID);
            _attachPartText.Write(vault, bw);
            _smackableCollisionNameText.Write(vault, bw);
            SmackableCollisionAttribute.Write(vault, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _markerNameText.ReadPointerData(vault, br);
            _attachPartText.ReadPointerData(vault, br);
            _smackableCollisionNameText.ReadPointerData(vault, br);

            MarkerName = _markerNameText.Value;
            AttachPart = _attachPartText.Value;
            SmackableCollisionName = _smackableCollisionNameText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _markerNameText.WritePointerData(vault, bw);
            _attachPartText.WritePointerData(vault, bw);
            _smackableCollisionNameText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _markerNameText.AddPointers(vault);
            _attachPartText.AddPointers(vault);
            _smackableCollisionNameText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _markerNameText.GetStrings().Concat(_attachPartText.GetStrings())
                .Concat(_smackableCollisionNameText.GetStrings());
        }

        public CCarDamageMarkerEntry(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CCarDamageMarkerEntry(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}