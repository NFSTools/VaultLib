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

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(CCarKitSlotEntry))]
    public class CCarKitSlotEntry : VLTBaseType, IReferencesStrings
    {
        public RefSpec Part { get; set; }
        public string SlotName { get; set; }

        private Text _slotNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            Part = new RefSpec(Class, Field, Collection);
            _slotNameText = new Text(Class, Field, Collection);

            Part.Read(vault, br);
            _slotNameText.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            Part.Write(vault, bw);
            _slotNameText.Write(vault, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _slotNameText.ReadPointerData(vault, br);
            SlotName = _slotNameText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _slotNameText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _slotNameText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _slotNameText.GetStrings();
        }

        public CCarKitSlotEntry(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public CCarKitSlotEntry(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}