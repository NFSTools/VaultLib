using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.Undercover.VLT
{
    [VltTypeInfo(nameof(TireTimeEffectRecord))]
    public class TireTimeEffectRecord : VltBaseType
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

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            mTireCondition = br.ReadEnum<TireCondition>();
            mEmitter.Read(context, br);
            mEmitterLowLod.Read(context, br);
            mMinTime = br.ReadSingle();
            mMaxTime = br.ReadSingle();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            bw.WriteEnum(mTireCondition);
            mEmitter.Write(context, bw);
            mEmitterLowLod.Write(context, bw);
            bw.Write(mMinTime);
            bw.Write(mMaxTime);
        }
    }
}