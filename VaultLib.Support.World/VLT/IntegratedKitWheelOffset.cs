// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 4:00 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT
{
    [VltTypeInfo(nameof(IntegratedKitWheelOffset))]
    public class IntegratedKitWheelOffset : VltBaseType, IReferencesStrings
    {
        //private Text _text;
        //private uint _kitHash;
        //private uint _u2;

        public string KitName { get; set; } = string.Empty;

        public uint Offset { get; set; }

        private Text _kitName { get; set; } = new();

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            br.ReadUInt32(); // stringhash32(KitName)
            _kitName.Read(context, fieldContext, br);
            Offset = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(Vlt32Hasher.Hash(KitName));
            _kitName.Value = KitName;
            _kitName.Write(context, fieldContext, bw);
            bw.Write(Offset);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { KitName };
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _kitName.ReadPointerData(context, fieldContext, br);
            KitName = _kitName.Value;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _kitName.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            _kitName.AddPointers(context, fieldContext);
        }
    }
}