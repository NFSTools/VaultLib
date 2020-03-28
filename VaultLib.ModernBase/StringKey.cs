// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:19 PM.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.ModernBase
{
    public class StringKey : VLTBaseType, IReferencesStrings, IStringValue
    {
        public string Value { get; set; }

        private Text _text;


        public override void Read(Vault vault, BinaryReader br)
        {
            br.ReadUInt32();
            _text.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _text.Value = Value;
            bw.Write(VLT32Hasher.Hash(Value));
            _text.Write(vault, bw);
        }

        public IEnumerable<string> GetStrings()
        {
            return new List<string>(new[] { Value });
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

        public override string ToString()
        {
            return Value;
        }

        public string GetString()
        {
            return Value;
        }

        public void SetString(string str)
        {
            Value = str;
        }

        public StringKey(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _text = new Text(Class, Field, Collection);
            Value = string.Empty;
        }
    }
}