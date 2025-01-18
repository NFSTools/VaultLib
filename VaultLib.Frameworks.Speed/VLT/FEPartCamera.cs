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

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(FEPartCamera))]
    public class FEPartCamera : VltBaseType, IReferencesStrings
    {
        public string SlotName { get; set; }
        public RefSpec Camera { get; set; }

        private Text _slotNameText;

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            _slotNameText.Read(context, br);
            Camera.Read(context, br);
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            _slotNameText.Write(context, bw);
            Camera.Write(context, bw);
        }

        public void ReadPointerData(VaultReadContext context, BinaryReader br)
        {
            _slotNameText.ReadPointerData(context, br);
            SlotName = _slotNameText.Value;
        }

        public void WritePointerData(VaultWriteContext context, BinaryWriter bw)
        {
            _slotNameText.Value = SlotName;
            _slotNameText.WritePointerData(context, bw);
        }

        public void AddPointers(VaultWriteContext context)
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
            SlotName = string.Empty;
            Camera = new RefSpec(Class, Field, Collection);
        }
    }
}