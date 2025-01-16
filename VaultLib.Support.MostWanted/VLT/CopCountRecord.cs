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

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            _copType.Read(context, br);
            Count = br.ReadUInt32();
            Chance = br.ReadUInt32();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            _copType.Value = CopType;
            _copType.Write(context, bw);
            bw.Write(Count);
            bw.Write(Chance);
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            _copType.ReadPointerData(context, br);
            CopType = _copType.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _copType.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            _copType.AddPointers(context);
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