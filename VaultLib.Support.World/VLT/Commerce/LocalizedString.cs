// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 9:27 AM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.Commerce;

[VltTypeInfo("Commerce::LocalizedString")]
public class LocalizedString: VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings<Core.DataInterfaces.Key32>, IStringValue
{
    public string Value { get; set; } = string.Empty;

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Value = context.ReadString(br);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        context.WriteString(Value, fieldContext, bw);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { Value };
    }

    public void ReadPointerData(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        //
    }

    public void WritePointerData(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        //
    }

    public void AddPointers(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext)
    {
        //
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