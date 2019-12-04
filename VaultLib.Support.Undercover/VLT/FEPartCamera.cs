// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:21 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(FEPartCamera))]
    public class FEPartCamera : VLTBaseType, IPointerObject, IReferencesStrings
    {
        public string SlotName { get; set; }
        public RefSpec Camera { get; set; }
        public RefSpec Camera_4_3 { get; set; }

        private Text _slotNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            _slotNameText = new Text(Class, Field, Collection);
            Camera = new RefSpec(Class, Field, Collection);
            Camera_4_3 = new RefSpec(Class, Field, Collection);

            _slotNameText.Read(vault, br);
            Camera.Read(vault, br);
            Camera_4_3.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _slotNameText.Write(vault, bw);
            Camera.Write(vault, bw);
            Camera_4_3.Write(vault, bw);
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

        public FEPartCamera(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public FEPartCamera(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}