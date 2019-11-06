// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:28 PM.

using System.Diagnostics;
using System.IO;

namespace VaultLib.Core.Types.EA.WorldMap
{
    public class MiniMapIconHash_W : VLTBaseType
    {
        public uint Hash { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Hash = br.ReadUInt32();
            Debug.WriteLine(Hash);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Hash);
        }
    }
}