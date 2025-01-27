// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:24 AM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.GameCore;

[VltTypeInfo("GameCore::KeyValuePair")]
public class KeyValuePair: VltBaseType<uint>, IReferencesStrings<uint>
{
    public string KeyString { get; set; } = string.Empty;

    public float Value { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        KeyString = context.ReadString(br);

        br.ReadUInt32(); // stringhash32(KeyString)
        Value = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        context.WriteString(KeyString, fieldContext, bw);
        bw.Write(Vlt32Hasher.Hash(KeyString));
        bw.Write(Value);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { KeyString };
    }

    public void ReadPointerData(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        //
    }

    public void WritePointerData(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        //
    }

    public void AddPointers(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext)
    {
        //
    }
}