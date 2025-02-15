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
public class KeyValuePair: VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings
{
    public string KeyString { get; set; } = string.Empty;

    public float Value { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        KeyString = context.ReadString(br);

        br.ReadUInt32(); // stringhash32(KeyString)
        Value = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        context.WriteString(KeyString, fieldContext, bw);
        bw.Write(Vlt32Hasher.Hash(KeyString));
        bw.Write(Value);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { KeyString };
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}