// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:31 PM.

using CoreLibraries.IO;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VltTypeInfo("GameCore::BehaviorSlot")]
    public class BehaviorSlot : VltBaseType
    {
        public enum BehaviorFlag
        {
            kBehavior_Activatable = 1,
            kBehavior_AutoActive
        }

        public uint mBehaviorChannel { get; set; }
        public uint mBehaviorType { get; set; }
        public BehaviorFlag mFlags { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            mBehaviorChannel = br.ReadUInt32();
            mBehaviorType = br.ReadUInt32();
            mFlags = br.ReadEnum<BehaviorFlag>();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(mBehaviorChannel);
            bw.Write(mBehaviorType);
            bw.WriteEnum(mFlags);
        }

        public BehaviorSlot(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public BehaviorSlot(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}