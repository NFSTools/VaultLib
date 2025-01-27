// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:27 AM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CopCountRecord))]
public class CopCountRecord: VltBaseType<uint>, IReferencesStrings<uint>
{
    public string CopType { get; set; } = string.Empty;

    public uint Count { get; set; }
    public uint Chance { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        CopType = context.ReadString(br);
        br.ReadUInt32();
        Count = br.ReadUInt32();
        Chance = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        context.WriteString(CopType, fieldContext, bw);
        bw.Write(Vlt32Hasher.Hash(CopType));
        bw.Write(Count);
        bw.Write(Chance);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { CopType };
    }

    public void ReadPointerData(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
    }

    public void WritePointerData(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
    }

    public void AddPointers(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext)
    {
    }
}