// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 4:00 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT;

[VltTypeInfo(nameof(IntegratedKitWheelOffset))]
public class IntegratedKitWheelOffset: VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings
{
    public string KitName { get; set; } = string.Empty;

    public uint Offset { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        br.ReadUInt32(); // stringhash32(KitName)
        KitName = context.ReadString(br);
        Offset = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Vlt32Hasher.Hash(KitName));
        context.WriteString(KitName, fieldContext, bw);
        bw.Write(Offset);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { KitName };
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}