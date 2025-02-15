// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:42 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("CCarSlotEntry")]
public class CCarSlotEntry : VltBaseType<Key32>, IReferencesStrings, IVltPointerObject<Key32>
{
    public DynamicSizeArray<Key32, RefSpec32> Parts { get; set; } = new();
    public string SlotName { get; set; } = string.Empty;

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        Parts.Read(context, fieldContext, br);
        SlotName = context.ReadString(br);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        Parts.Write(context, fieldContext, bw);
        context.WriteString(SlotName, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        Parts.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        Parts.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext)
    {
        Parts.AddPointers(context, fieldContext);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { SlotName };
    }

    public override object Clone()
    {
        return new CCarSlotEntry
        {
            Parts = (DynamicSizeArray<Key32, RefSpec32>)this.Parts.Clone(),
            SlotName = this.SlotName,
        };
    }
}