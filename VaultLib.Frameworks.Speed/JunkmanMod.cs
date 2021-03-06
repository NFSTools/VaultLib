﻿// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:38 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(JunkmanMod))]
    public class JunkmanMod : VLTBaseType
    {
        public uint ClassKey { get; set; }
        public uint DefinitionKey { get; set; }
        public float ScaleF { get; set; }
        public float ScaleR { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            ClassKey = br.ReadUInt32();
            DefinitionKey = br.ReadUInt32();
            ScaleF = br.ReadSingle();
            ScaleR = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(ClassKey);
            bw.Write(DefinitionKey);
            bw.Write(ScaleF);
            bw.Write(ScaleR);
        }

        public JunkmanMod(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public JunkmanMod(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}