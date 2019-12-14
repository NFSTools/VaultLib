// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:59 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    public class FEPartDetail : VLTBaseType
    {
        public uint Logo { get; set; }
        public uint Name { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Logo = br.ReadUInt32();
            Name = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Logo);
            bw.Write(Name);
        }

        public FEPartDetail(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FEPartDetail(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}