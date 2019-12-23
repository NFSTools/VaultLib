// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:34 PM.

using CoreLibraries.IO;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::BlackBoardSlot")]
    public class BlackBoardSlot : VLTBaseType
    {
        public enum BlackBoardFlag
        {
            kBlackBoardFlag_Loading = 1,
            kBlackBoardFlag_Running = 2,
            kBlackBoardFlag_Countdown = 4
        }

        public BlackBoardChannel mChannel { get; set; }
        public uint mBlackBoardKey { get; set; }
        public BlackBoardFlag mFlag { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            mChannel = br.ReadEnum<BlackBoardChannel>();
            mBlackBoardKey = br.ReadUInt32();
            mFlag = br.ReadEnum<BlackBoardFlag>();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(mChannel);
            bw.Write(mBlackBoardKey);
            bw.WriteEnum(mFlag);
        }

        public BlackBoardSlot(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public BlackBoardSlot(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}