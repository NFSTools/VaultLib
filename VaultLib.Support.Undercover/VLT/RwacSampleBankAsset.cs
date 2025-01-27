// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 3:43 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(RwacSampleBankAsset))]
public class RwacSampleBankAsset: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public uint Bank { get; set; }
    public uint Asset { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Bank = br.ReadUInt32();
        Asset = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Bank);
        bw.Write(Asset);
    }

    public override string ToString()
    {
        return $"RWAC Bank {Bank:X8} -> Asset {Asset:X8}";
    }
}