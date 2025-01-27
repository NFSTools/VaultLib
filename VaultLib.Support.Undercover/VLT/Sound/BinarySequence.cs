// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 9:00 PM.

using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.Sound;

[VltTypeInfo("Sound::BinarySequence")]
public class BinarySequence: VltBaseType<uint>
{
    public bool Value { get; set; }
    public float Duration { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Value = br.ReadBoolean();
        br.SafeAlignReader(4);
        Duration = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(Value);
        bw.AlignWriter(4);
        bw.Write(Duration);
    }
}