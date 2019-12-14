// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:46 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::StringKey")]
    public class StringKey : VLTBaseType
    {
        public uint Key { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Key = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Key);
        }

        public StringKey(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public StringKey(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}