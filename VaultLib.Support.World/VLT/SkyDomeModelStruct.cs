// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 7:23 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    public class SkyDomeModelStruct : VLTBaseType
    {
        public uint Hash { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Hash = br.ReadUInt32(); // SKYDOME_1_DAWN, SKYDOME_1_DUSK, etc
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Hash);
        }
    }
}