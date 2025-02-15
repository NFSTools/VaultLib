// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:27 AM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CopCountRecord))]
public class CopCountRecord : VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings
{
    public string CopType { get; set; } = string.Empty;

    public uint Count { get; set; }
    public uint Chance { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        CopType = context.ReadString(br);
        br.ReadUInt32();
        Count = br.ReadUInt32();
        Chance = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        context.WriteString(CopType, fieldContext, bw);
        bw.Write(Vlt32Hasher.Hash(CopType));
        bw.Write(Count);
        bw.Write(Chance);
    }

    public override object Clone()
    {
        return new CopCountRecord
        {
            CopType = CopType,
            Count = Count,
            Chance = Chance
        };
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { CopType };
    }
}