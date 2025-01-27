// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:19 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.LegacyBase;

public class StringKey64 : VltBaseType<VaultLib.Core.DataInterfaces.Key32>, IReferencesStrings<VaultLib.Core.DataInterfaces.Key32>, IStringValue
{
    public string Value { get; set; } = string.Empty;

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        br.ReadInt64();
        br.ReadUInt32();
        Value = context.ReadString(br);
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Vlt64Hasher.Hash(Value));
        bw.Write(Vlt32Hasher.Hash(Value));
        context.WriteString(Value, fieldContext, bw);
    }

    public IEnumerable<string> GetStrings()
    {
        return new List<string>(new[] { Value });
    }

    public void ReadPointerData(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
    }

    public void WritePointerData(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
    }

    public void AddPointers(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext)
    {
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
}