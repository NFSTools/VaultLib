// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:38 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.MostWanted.VLT
{
    [VltTypeInfo(nameof(JunkmanMod))]
    public class JunkmanMod : VltBaseType
    {
        public uint ClassKey { get; set; }
        public uint DefinitionKey { get; set; }
        public float ScaleF { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            ClassKey = br.ReadUInt32();
            DefinitionKey = br.ReadUInt32();
            ScaleF = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(ClassKey);
            bw.Write(DefinitionKey);
            bw.Write(ScaleF);
        }
    }
}