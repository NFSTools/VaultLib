// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:33 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(TargetTimeOverrides))]
public class TargetTimeOverrides : VltBaseType<Key32>, IReferencesStrings
{
    public RefSpec32 Car { get; set; } = new();
    public string Event { get; set; } = string.Empty;
    public float MinDelta { get; set; }
    public float MaxDelta { get; set; }
    public float Shift { get; set; }

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        Car.Read(context, fieldContext, br);
        Event = context.ReadString(br);
        MinDelta = br.ReadSingle();
        MaxDelta = br.ReadSingle();
        Shift = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        Car.Write(context, fieldContext, bw);
        context.WriteString(Event, fieldContext, bw);
        bw.Write(MinDelta);
        bw.Write(MaxDelta);
        bw.Write(Shift);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { Event };
    }

    public override object Clone()
    {
        return new TargetTimeOverrides
        {
            Car = (RefSpec32)this.Car.Clone(),
            Event = this.Event,
            MinDelta = this.MinDelta,
            MaxDelta = this.MaxDelta,
            Shift = this.Shift,
        };
    }
}