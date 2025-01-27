// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:42 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("CCarSlotEntry")]
public class CCarSlotEntry : VltBaseType<uint>, IReferencesStrings<uint>
{
    public DynamicSizeArray<uint, RefSpec<uint>> Parts { get; set; } = new();
    public string SlotName { get; set; } = string.Empty;

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Parts.Read(context, fieldContext, br);
        SlotName = context.ReadString(br);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext,
        BinaryWriter bw)
    {
        Parts.Write(context, fieldContext, bw);
        context.WriteString(SlotName, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Parts.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext,
        BinaryWriter bw)
    {
        Parts.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext)
    {
        Parts.AddPointers(context, fieldContext);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { SlotName };
    }
}