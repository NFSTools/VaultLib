// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 7:55 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.LegacyBase;

namespace VaultLib.Support.MostWanted.VLT
{
    [VLTTypeInfo(nameof(CopCountRecord))]
    public class CopCountRecord : VLTBaseType, IReferencesStrings
    {
        public string CopType { get; set; }
        public uint Count { get; set; }
        public uint Chance { get; set; }

        private StringKey64 _copType;

        public override void Read(Vault vault, BinaryReader br)
        {
            _copType.Read(vault, br);
            Count = br.ReadUInt32();
            Chance = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _copType.Value = CopType;
            _copType.Write(vault, bw);
            bw.Write(Count);
            bw.Write(Chance);
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

        public IEnumerable<string> GetStrings()
        {
            return new[] { CopType };
        }

        public CopCountRecord(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _copType = new StringKey64(Class, Field, Collection);
            CopType = string.Empty;
        }
    }
}