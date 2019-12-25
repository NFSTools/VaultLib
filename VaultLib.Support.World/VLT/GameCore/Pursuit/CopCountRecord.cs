﻿// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:27 AM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.GameCore.Pursuit
{
    [VLTTypeInfo("GameCore::Pursuit::CopCountRecord")]
    public class CopCountRecord : VLTBaseType, IReferencesStrings
    {
        private Text _copType;

        public string CopType { get; set; }

        public uint Count { get; set; }
        public uint Chance { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            _copType = new Text(Class, Field, Collection);
            _copType.Read(vault, br);
            br.ReadUInt32();
            Count = br.ReadUInt32();
            Chance = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _copType = new Text(Class, Field, Collection) { Value = CopType };
            _copType.Write(vault, bw);
            bw.Write(string.IsNullOrEmpty(CopType) ? 0 : VLT32Hasher.Hash(CopType));
            bw.Write(Count);
            bw.Write(Chance);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { CopType };
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _copType.ReadPointerData(vault, br);
            CopType = _copType.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _copType.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _copType.AddPointers(vault);
        }

        public CopCountRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CopCountRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}