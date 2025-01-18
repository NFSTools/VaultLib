using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.World.VLT
{
    [VltTypeInfo(nameof(TireEffectRecord))]
    public class TireEffectRecord : VltBaseType
    {
        public TireEffectRecord(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            mEmitter = new RefSpecPacked(Class, Field, Collection);
        }

        public TireCondition mTireCondition { get; set; }
        public RefSpecPacked mEmitter { get; set; }
        public float mMinSpeed { get; set; }
        public float mMaxSpeed { get; set; }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            mEmitter.Read(context, br);
            mTireCondition = br.ReadEnum<TireCondition>();
            mMinSpeed = br.ReadSingle();
            mMaxSpeed = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            mEmitter.Write(context, bw);
            bw.WriteEnum(mTireCondition);
            bw.Write(mMinSpeed);
            bw.Write(mMaxSpeed);
        }
    }
}