// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 7:55 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.LegacyBase;

namespace VaultLib.Support.MostWanted.VLT;

[VltTypeInfo(nameof(CopCountRecord))]
public class CopCountRecord : VltBaseType, IReferencesStrings
{
    public string CopType { get; set; } = string.Empty;
    public uint Count { get; set; }
    public uint Chance { get; set; }

    private StringKey64 _copType = new();

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        _copType.Read(context, fieldContext, br);
        Count = br.ReadUInt32();
        Chance = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        _copType.Value = CopType;
        _copType.Write(context, fieldContext, bw);
        bw.Write(Count);
        bw.Write(Chance);
    }

    public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        _copType.ReadPointerData(context, fieldContext, br);
        CopType = _copType.Value;
    }

    public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        _copType.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
    {
        _copType.AddPointers(context, fieldContext);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { CopType };
    }
}