// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:28 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.EA.WorldMap
{
    [VltTypeInfo("EA::WorldMap::MiniMapIconHash")]
    public class MiniMapIconHash : VltBaseType
    {
        public uint Hash { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Hash = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(Hash);
        }

        public MiniMapIconHash(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public MiniMapIconHash(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}