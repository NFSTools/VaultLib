// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:46 AM.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.NIS;

[VltTypeInfo("NIS::NISActor")]
public class NISActor : VltBaseType, IReferencesStrings
{
    public string ActorName { get; set; } = string.Empty;
    public string CarChannelName { get; set; } = string.Empty;
    public bool IsDriver { get; set; }
    public float ExitAnimSec { get; set; }
    public bool IsFacePixelation { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        ActorName = context.ReadString(br);
        CarChannelName = context.ReadString(br);
        IsDriver = br.ReadBoolean();
        br.AlignReader(4);
        ExitAnimSec = br.ReadSingle();
        IsFacePixelation = br.ReadBoolean();
        br.AlignReader(4);
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        context.WriteString(ActorName, fieldContext, bw);
        context.WriteString(CarChannelName, fieldContext, bw);
        bw.Write(IsDriver);
        bw.AlignWriter(4);
        bw.Write(ExitAnimSec);
        bw.Write(IsFacePixelation);
        bw.AlignWriter(4);
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
        return new[] { ActorName, CarChannelName };
    }
}