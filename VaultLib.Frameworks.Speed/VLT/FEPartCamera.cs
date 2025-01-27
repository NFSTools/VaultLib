// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:21 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FEPartCamera))]
public class FEPartCamera: VltBaseType<uint>, IReferencesStrings<uint>
{
    public string SlotName { get; set; } = string.Empty;
    public RefSpec<uint> Camera { get; set; } = new();

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        SlotName = context.ReadString(br);
        Camera.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        context.WriteString(SlotName, fieldContext, bw);
        Camera.Write(context, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
    }

    public void WritePointerData(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
    }

    public void AddPointers(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext)
    {
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { SlotName };
    }
}