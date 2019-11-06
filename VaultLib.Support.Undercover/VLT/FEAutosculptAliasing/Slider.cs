// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:02 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.FEAutosculptAliasing
{
    public class Slider : VLTBaseType
    {
        public uint Region { get; set; }
        public uint Zone { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Region = br.ReadUInt32();
            Zone = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Region);
            bw.Write(Zone);
        }
    }
}