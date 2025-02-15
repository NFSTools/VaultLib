// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:46 AM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.NIS;

[VltTypeInfo("NIS::NISActor")]
public class NISActor: VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings
{
    public string ActorName { get; set; } = string.Empty;
    public string CarChannelName { get; set; } = string.Empty;
    public bool IsDriver { get; set; }
    public float ExitAnimSec { get; set; }
    public bool IsFacePixelation { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        ActorName = context.ReadString(br);
        CarChannelName = context.ReadString(br);
        IsDriver = br.ReadBoolean();
        br.SafeAlignReader(4);
        ExitAnimSec = br.ReadSingle();
        IsFacePixelation = br.ReadBoolean();
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        context.WriteString(ActorName, fieldContext, bw);
        context.WriteString(CarChannelName, fieldContext, bw);
        bw.Write(IsDriver);
        bw.AlignWriter(4);
        bw.Write(ExitAnimSec);
        bw.Write(IsFacePixelation);
        bw.AlignWriter(4);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { ActorName, CarChannelName };
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}