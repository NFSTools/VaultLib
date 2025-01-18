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
    [VltTypeInfo("GameCore::KeyValuePair")]
    public class KeyValuePair : VltBaseType, IReferencesStrings
    {
        private Text _keyString;

        public string KeyString { get; set; }

        public float Value { get; set; }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            _keyString.Read(context, br);

            br.ReadUInt32(); // stringhash32(KeyString)
            Value = br.ReadSingle();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            _keyString.Value = KeyString;
            _keyString.Write(context, bw);
            bw.Write(Vlt32Hasher.Hash(KeyString));
            bw.Write(Value);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { KeyString };
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            _keyString.ReadPointerData(context, br);
            KeyString = _keyString.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _keyString.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            _keyString.AddPointers(context);
        }

        public KeyValuePair(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _keyString = new Text(Class, Field, Collection);
            KeyString = string.Empty;
        }
    }
}