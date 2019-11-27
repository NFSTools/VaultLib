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
    [VLTTypeInfo("Commerce::LocalizedString")]
    public class LocalizedString : VLTBaseType, IReferencesStrings, IStringValue
    {
        private Text _text;

        public string Value { get; set; }

        public LocalizedString()
        {
            Value = string.Empty;
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            _text = new Text();
            _text.Collection = Collection;
            _text.Class = Class;
            _text.Field = Field;
            _text.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _text = new Text();
            _text.Collection = Collection;
            _text.Class = Class;
            _text.Field = Field;
            _text.Value = Value;
            _text.Write(vault, bw);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] {Value};
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _text.ReadPointerData(vault, br);
            Value = _text.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _text.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _text.AddPointers(vault);
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