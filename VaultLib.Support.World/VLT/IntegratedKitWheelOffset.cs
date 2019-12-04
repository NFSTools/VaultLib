// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 4:00 PM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.GameUtilities;
using VaultLib.Core;
using VaultLib.Core.Data;
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

        public override void Read(Vault vault, BinaryReader br)
        {
            br.ReadUInt32(); // stringhash32(KitName)
            _kitName = new Text(Class, Field, Collection);
            _kitName.Read(vault, br);
            Offset = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            if (string.IsNullOrEmpty(KitName))
                bw.Write(0);
            else
                bw.Write(VLT32Hasher.Hash(KitName));
            _kitName = new Text(Class, Field, Collection) { Value = KitName };
            _kitName.Write(vault, bw);
            bw.Write(Offset);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] {KitName};
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _kitName.ReadPointerData(vault, br);
            KitName = _kitName.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _kitName.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _kitName.AddPointers(vault);
        }

        public IntegratedKitWheelOffset(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public IntegratedKitWheelOffset(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}