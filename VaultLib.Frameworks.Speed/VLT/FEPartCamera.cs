// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:21 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FEPartCamera))]
public class FEPartCamera : VltBaseType<Key32>, IReferencesStrings
{
    public string SlotName { get; set; } = string.Empty;
    public RefSpec32 Camera { get; set; } = new();

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        SlotName = context.ReadString(br);
        Camera.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        context.WriteString(SlotName, fieldContext, bw);
        Camera.Write(context, fieldContext, bw);
    }

    public override object Clone()
    {
        return new FEPartCamera
        {
            SlotName = SlotName,
            Camera = (RefSpec32)Camera.Clone(),
        };
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { SlotName };
    }
}