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
    [VltTypeInfo(nameof(CCarDamageMarkerEntry))]
    public class CCarDamageMarkerEntry : VltBaseType, IReferencesStrings
    {
        public string MarkerName { get; set; } = string.Empty;
        public int PartID { get; set; }
        public int SlotID { get; set; }
        public string AttachPart { get; set; } = string.Empty;
        public string SmackableCollisionName { get; set; } = string.Empty;
        public RefSpec SmackableCollisionAttribute { get; set; } = new();

        private Text _markerNameText = new();
        private Text _attachPartText = new();
        private Text _smackableCollisionNameText = new();

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _markerNameText.Read(context, fieldContext, br);
            PartID = br.ReadInt32();
            SlotID = br.ReadInt32();
            _attachPartText.Read(context, fieldContext, br);
            _smackableCollisionNameText.Read(context, fieldContext, br);
            SmackableCollisionAttribute.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _markerNameText.Value = MarkerName;
            _markerNameText.Write(context, fieldContext, bw);
            bw.Write(PartID);
            bw.Write(SlotID);
            _attachPartText.Value = AttachPart;
            _attachPartText.Write(context, fieldContext, bw);
            _smackableCollisionNameText.Value = SmackableCollisionName;
            _smackableCollisionNameText.Write(context, fieldContext, bw);
            SmackableCollisionAttribute.Write(context, fieldContext, bw);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _markerNameText.ReadPointerData(context, fieldContext, br);
            _attachPartText.ReadPointerData(context, fieldContext, br);
            _smackableCollisionNameText.ReadPointerData(context, fieldContext, br);

            MarkerName = _markerNameText.Value;
            AttachPart = _attachPartText.Value;
            SmackableCollisionName = _smackableCollisionNameText.Value;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _markerNameText.WritePointerData(context, fieldContext, bw);
            _attachPartText.WritePointerData(context, fieldContext, bw);
            _smackableCollisionNameText.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            _markerNameText.AddPointers(context, fieldContext);
            _attachPartText.AddPointers(context, fieldContext);
            _smackableCollisionNameText.AddPointers(context, fieldContext);
        }

        public IEnumerable<string> GetStrings()
        {
            return _markerNameText.GetStrings().Concat(_attachPartText.GetStrings())
                .Concat(_smackableCollisionNameText.GetStrings());
        }
    }
}