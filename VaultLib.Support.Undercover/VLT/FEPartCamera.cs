// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:21 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(FEPartCamera))]
public class FEPartCamera: VltBaseType<VaultLib.Core.DataInterfaces.Key32>, IReferencesStrings<VaultLib.Core.DataInterfaces.Key32>
{
    public string SlotName { get; set; } = string.Empty;
    public RefSpec<VaultLib.Core.DataInterfaces.Key32> Camera { get; set; } = new();
    public RefSpec<VaultLib.Core.DataInterfaces.Key32> Camera_4_3 { get; set; } = new();

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        SlotName = context.ReadString(br);
        Camera.Read(context, fieldContext, br);
        Camera_4_3.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        context.WriteString(SlotName, fieldContext, bw);
        Camera.Write(context, fieldContext, bw);
        Camera_4_3.Write(context, fieldContext, bw);
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

    public IEnumerable<string> GetStrings()
    {
        return new[] { SlotName };
    }
}