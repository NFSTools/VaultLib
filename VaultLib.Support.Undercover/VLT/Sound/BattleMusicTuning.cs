// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 7:26 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.Sound;

[VltTypeInfo("Sound::BattleMusicTuning")]
public class BattleMusicTuning: VltBaseType<Core.DataInterfaces.Key32>
{
    public float TimeAhead_HiToMed { get; set; }
    public float TimeAhead_MedToLo { get; set; }
    public float TimeBehind_HiToFail { get; set; }
    public float TimeAhead_FailToHi { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        TimeAhead_HiToMed = br.ReadSingle();
        TimeAhead_MedToLo = br.ReadSingle();
        TimeBehind_HiToFail = br.ReadSingle();
        TimeAhead_FailToHi = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(TimeAhead_HiToMed);
        bw.Write(TimeAhead_MedToLo);
        bw.Write(TimeBehind_HiToFail);
        bw.Write(TimeAhead_FailToHi);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}