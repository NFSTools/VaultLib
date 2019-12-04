﻿// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:38 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.MostWanted.VLT
{
    [VLTTypeInfo(nameof(JunkmanMod))]
    public class JunkmanMod : VLTBaseType
    {
        public uint ClassKey { get; set; }
        public uint DefinitionKey { get; set; }
        public float ScaleF { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            ClassKey = br.ReadUInt32();
            DefinitionKey = br.ReadUInt32();
            ScaleF = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(ClassKey);
            bw.Write(DefinitionKey);
            bw.Write(ScaleF);
        }

        public JunkmanMod(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public JunkmanMod(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}