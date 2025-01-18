using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT
{
    [VltTypeInfo(nameof(TireEffectRecord))]
    public class TireEffectRecord : VltBaseType
    {
        public TireEffectRecord(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            mEmitter = new RefSpec(Class, Field, Collection);
            mEmitterLowLod = new RefSpec(Class, Field, Collection);
        }

        public TireCondition mTireCondition { get; set; }
        public RefSpec mEmitter { get; set; }
        public RefSpec mEmitterLowLod { get; set; }
        public float mMinSpeed { get; set; }
        public float mMaxSpeed { get; set; }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            mTireCondition = br.ReadEnum<TireCondition>();
            mEmitter.Read(context, br);
            mEmitterLowLod.Read(context, br);
            mMinSpeed = br.ReadSingle();
            mMaxSpeed = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.WriteEnum(mTireCondition);
            mEmitter.Write(context, bw);
            mEmitterLowLod.Write(context, bw);
            bw.Write(mMinSpeed);
            bw.Write(mMaxSpeed);
        }
    }
}