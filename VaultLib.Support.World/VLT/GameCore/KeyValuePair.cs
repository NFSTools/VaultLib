// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:24 AM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::KeyValuePair")]
    public class KeyValuePair : VLTBaseType, IReferencesStrings
    {
        private Text _keyString;

        public string KeyString { get; set; }

        public float Value { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            _keyString.Read(vault, br);

            br.ReadUInt32(); // stringhash32(KeyString)
            Value = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _keyString.Value = KeyString;
            _keyString.Write(vault, bw);
            bw.Write(VLT32Hasher.Hash(KeyString));
            bw.Write(Value);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { KeyString };
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _keyString.ReadPointerData(vault, br);
            KeyString = _keyString.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _keyString.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _keyString.AddPointers(vault);
        }

        public KeyValuePair(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _keyString = new Text(Class, Field, Collection);
            KeyString = string.Empty;
        }
    }
}