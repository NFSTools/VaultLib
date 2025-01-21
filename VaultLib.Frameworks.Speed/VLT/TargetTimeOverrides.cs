// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:33 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(TargetTimeOverrides))]
public class TargetTimeOverrides : VltBaseType, IReferencesStrings
{
    public RefSpec Car { get; set; } = new();
    public string Event { get; set; } = string.Empty;
    public float MinDelta { get; set; }
    public float MaxDelta { get; set; }
    public float Shift { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        Car.Read(context, fieldContext, br);
        Event = context.ReadString(br);
        MinDelta = br.ReadSingle();
        MaxDelta = br.ReadSingle();
        Shift = br.ReadSingle();
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        Car.Write(context, fieldContext, bw);
        context.WriteString(Event, fieldContext, bw);
        bw.Write(MinDelta);
        bw.Write(MaxDelta);
        bw.Write(Shift);
    }

    public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
    }

    public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
    }

    public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
    {
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { Event };
    }
}