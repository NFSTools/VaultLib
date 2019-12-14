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

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(CCarSlotEntry))]
    public class CCarSlotEntry : VLTBaseType, IReferencesStrings
    {
        public DynamicSizeArray<RefSpec> Parts { get; set; }
        public string SlotName { get; set; }

        private Text _slotNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            Parts = new DynamicSizeArray<RefSpec>(Class, Field, Collection);
            Parts.Read(vault, br);
            _slotNameText = new Text(Class, Field, Collection);
            _slotNameText.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            Parts.Write(vault, bw);
            _slotNameText.Write(vault, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            Parts.ReadPointerData(vault, br);
            _slotNameText.ReadPointerData(vault, br);
            SlotName = _slotNameText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            Parts.WritePointerData(vault, bw);
            _slotNameText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            Parts.AddPointers(vault);
            _slotNameText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _slotNameText.GetStrings();
        }

        public CCarSlotEntry(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CCarSlotEntry(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}