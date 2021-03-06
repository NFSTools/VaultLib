﻿// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 3:43 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(RwacSampleBankAsset))]
    public class RwacSampleBankAsset : VLTBaseType
    {
        public uint Bank { get; set; }
        public uint Asset { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Bank = br.ReadUInt32();
            Asset = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Bank);
            bw.Write(Asset);
        }

        public override string ToString()
        {
            return $"RWAC Bank {Bank:X8} -> Asset {Asset:X8}";
        }

        public RwacSampleBankAsset(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public RwacSampleBankAsset(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}