// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:59 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    public class FEPartDetail : VltBaseType
    {
        public uint Logo { get; set; }
        public uint Name { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Logo = br.ReadUInt32();
            Name = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(Logo);
            bw.Write(Name);
        }
    }
}