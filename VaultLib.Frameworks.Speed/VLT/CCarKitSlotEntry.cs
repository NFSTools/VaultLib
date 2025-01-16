// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:38 PM.

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
    [VLTTypeInfo(nameof(CCarKitSlotEntry))]
    public class CCarKitSlotEntry : VLTBaseType, IReferencesStrings
    {
        public RefSpec Part { get; set; }
        public string SlotName { get; set; }

        private Text _slotNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            Part.Read(vault, br);
            _slotNameText.Read(vault, br);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            Part.Write(context, bw);
            _slotNameText.Write(context, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _slotNameText.ReadPointerData(vault, br);
            SlotName = _slotNameText.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _slotNameText.Value = SlotName;
            _slotNameText.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            _slotNameText.AddPointers(context);
        }

        public IEnumerable<string> GetStrings()
        {
            return _slotNameText.GetStrings();
        }

        public CCarKitSlotEntry(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Part = new RefSpec(Class, Field, Collection);
            _slotNameText = new Text(Class, Field, Collection);
            SlotName = string.Empty;
        }
    }
}