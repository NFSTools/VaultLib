// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:02 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.FEAutosculptAliasing
{
    public class Slider : VltBaseType
    {
        public uint Region { get; set; }
        public uint Zone { get; set; }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            Region = br.ReadUInt32();
            Zone = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.Write(Region);
            bw.Write(Zone);
        }

        public Slider(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Slider(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}