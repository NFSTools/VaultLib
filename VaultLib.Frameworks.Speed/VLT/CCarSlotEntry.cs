// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:42 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(CCarSlotEntry))]
    public class CCarSlotEntry : VltBaseType, IReferencesStrings
    {
        public DynamicSizeArray<RefSpec> Parts { get; set; }
        public string SlotName { get; set; }

        private Text _slotNameText;

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Parts.Read(context, fieldContext, br);
            _slotNameText.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            Parts.Write(context, fieldContext, bw);
            _slotNameText.Write(context, fieldContext, bw);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Parts.ReadPointerData(context, fieldContext, br);
            _slotNameText.ReadPointerData(context, fieldContext, br);
            SlotName = _slotNameText.Value;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            Parts.WritePointerData(context, fieldContext, bw);
            _slotNameText.Value = SlotName;
            _slotNameText.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            Parts.AddPointers(context, fieldContext);
            _slotNameText.AddPointers(context, fieldContext);
        }

        public IEnumerable<string> GetStrings()
        {
            return _slotNameText.GetStrings();
        }

        public CCarSlotEntry()
        {
            Parts = new DynamicSizeArray<RefSpec>();
            _slotNameText = new Text();
            SlotName = string.Empty;
        }
    }
}