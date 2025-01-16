// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 4:00 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(IntegratedKitWheelOffset))]
    public class IntegratedKitWheelOffset : VLTBaseType, IReferencesStrings
    {
        //private Text _text;
        //private uint _kitHash;
        //private uint _u2;

        public string KitName { get; set; }

        public uint Offset { get; set; }

        private Text _kitName { get; set; }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            br.ReadUInt32(); // stringhash32(KitName)
            _kitName.Read(context, br);
            Offset = br.ReadUInt32();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            bw.Write(VLT32Hasher.Hash(KitName));
            _kitName.Value = KitName;
            _kitName.Write(context, bw);
            bw.Write(Offset);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { KitName };
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            _kitName.ReadPointerData(context, br);
            KitName = _kitName.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _kitName.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            _kitName.AddPointers(context);
        }

        public IntegratedKitWheelOffset(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _kitName = new Text(Class, Field, Collection);
            KitName = string.Empty;
        }
    }
}