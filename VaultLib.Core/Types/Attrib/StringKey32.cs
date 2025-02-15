// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:19 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Hashing;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Attrib;

public class StringKey32 : VltBaseType<Key32>, IReferencesStrings, IStringValue
{
    public string Value { get; set; } = string.Empty;


    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        br.ReadUInt32();
        Value = context.ReadString(br);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        bw.Write(Vlt32Hasher.Hash(Value));
        context.WriteString(Value, fieldContext, bw);
    }

    public IEnumerable<string> GetStrings()
    {
        return new List<string>(new[] { Value });
    }

    public override string ToString()
    {
        return Value;
    }

    public string GetString()
    {
        return Value;
    }

    public void SetString(string str)
    {
        Value = str;
    }

    public override object Clone()
    {
        return new StringKey32
        {
            Value = Value,
        };
    }
}