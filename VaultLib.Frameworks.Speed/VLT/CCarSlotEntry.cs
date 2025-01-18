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

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            Parts.Read(context, br);
            _slotNameText.Read(context, br);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            Parts.Write(context, bw);
            _slotNameText.Write(context, bw);
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            Parts.ReadPointerData(context, br);
            _slotNameText.ReadPointerData(context, br);
            SlotName = _slotNameText.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            Parts.WritePointerData(context, bw);
            _slotNameText.Value = SlotName;
            _slotNameText.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            Parts.AddPointers(context);
            _slotNameText.AddPointers(context);
        }

        public IEnumerable<string> GetStrings()
        {
            return _slotNameText.GetStrings();
        }

        public CCarSlotEntry(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Parts = new DynamicSizeArray<RefSpec>(Class, Field, Collection);
            _slotNameText = new Text(Class, Field, Collection);
            SlotName = string.Empty;
        }
    }
}