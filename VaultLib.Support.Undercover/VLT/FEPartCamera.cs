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

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            _slotNameText.Read(context, br);
            Camera.Read(context, br);
            Camera_4_3.Read(context, br);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            _slotNameText.Value = SlotName;
            _slotNameText.Write(context, bw);
            Camera.Write(context, bw);
            Camera_4_3.Write(context, bw);
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            _slotNameText.ReadPointerData(context, br);
            SlotName = _slotNameText.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
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

        public FEPartCamera(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _slotNameText = new Text(Class, Field, Collection);
            Camera = new RefSpec(Class, Field, Collection);
            Camera_4_3 = new RefSpec(Class, Field, Collection);
            SlotName = string.Empty;
        }
    }
}