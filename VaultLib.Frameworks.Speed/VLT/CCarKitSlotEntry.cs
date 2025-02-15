// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:38 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("CCarKitSlotEntry")]
public class CCarKitSlotEntry : VltBaseType<Key32>, IReferencesStrings
{
    public RefSpec32 Part { get; set; } = new();
    public string SlotName { get; set; } = string.Empty;

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext, BinaryReader br)
    {
        Part.Read(context, fieldContext, br);
        SlotName = context.ReadString(br);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext, BinaryWriter bw)
    {
        Part.Write(context, fieldContext, bw);
        context.WriteString(SlotName, fieldContext, bw);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { SlotName };
    }

    public override object Clone()
    {
        return new CCarKitSlotEntry
        {
            Part = (RefSpec32)this.Part.Clone(),
            SlotName = this.SlotName,
        };
    }
}