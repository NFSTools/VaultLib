// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:28 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.EA.WorldMap
{
    [VLTTypeInfo("EA::WorldMap::MiniMapIconHash")]
    public class MiniMapIconHash : VLTBaseType
    {
        public uint Hash { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Hash = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Hash);
        }
    }
}