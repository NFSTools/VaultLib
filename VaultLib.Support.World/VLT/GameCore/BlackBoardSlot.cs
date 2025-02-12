// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:34 PM.

using System;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore;

[VltTypeInfo("GameCore::BlackBoardSlot")]
public class BlackBoardSlot : VltBaseType<Core.DataInterfaces.Key32>
{
    [Flags]
    public enum BlackBoardFlags
    {
        kBlackBoardFlag_Loading = 1,
        kBlackBoardFlag_Running = 2,
        kBlackBoardFlag_Countdown = 4
    }

    public BlackBoardChannel mChannel { get; set; }
    public Key32 mBlackBoardKey { get; set; }
    public BlackBoardFlags mFlag { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        mChannel = br.ReadEnum<BlackBoardChannel>();
        mBlackBoardKey = Key32.Read(br);
        mFlag = br.ReadEnum<BlackBoardFlags>();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(mChannel);
        mBlackBoardKey.Write(bw);
        bw.WriteEnum(mFlag);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}