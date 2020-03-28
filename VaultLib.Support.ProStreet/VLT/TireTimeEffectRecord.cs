﻿using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(TireTimeEffectRecord))]
    public class TireTimeEffectRecord : VLTBaseType
    {
        public TireTimeEffectRecord(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            mEmitter = new RefSpec(Class, Field, Collection);
            mEmitterLowLod = new RefSpec(Class, Field, Collection);
        }

        public TireCondition mTireCondition { get; set; }
        public RefSpec mEmitter { get; set; }
        public RefSpec mEmitterLowLod { get; set; }
        public float mMinTime { get; set; }
        public float mMaxTime { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            mTireCondition = br.ReadEnum<TireCondition>();
            mEmitter.Read(vault, br);
            mEmitterLowLod.Read(vault, br);
            mMinTime = br.ReadSingle();
            mMaxTime = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(mTireCondition);
            mEmitter.Write(vault, bw);
            mEmitterLowLod.Write(vault, bw);
            bw.Write(mMinTime);
            bw.Write(mMaxTime);
        }
    }
}