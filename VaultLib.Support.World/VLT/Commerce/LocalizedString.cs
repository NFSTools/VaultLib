// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 9:27 AM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.Commerce
{
    [VltTypeInfo("Commerce::LocalizedString")]
    public class LocalizedString : VltBaseType, IReferencesStrings, IStringValue
    {
        private Text _text = new();

        public string Value { get; set; } = string.Empty;

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _text.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _text.Value = Value;
            _text.Write(context, fieldContext, bw);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { Value };
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _text.ReadPointerData(context, fieldContext, br);
            Value = _text.Value;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _text.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            _text.AddPointers(context, fieldContext);
        }

        public string GetString()
        {
            return Value;
        }

        public void SetString(string str)
        {
            Value = str;
        }
    }
}