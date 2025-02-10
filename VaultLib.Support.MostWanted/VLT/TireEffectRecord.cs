using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.MostWanted.VLT;

[VltTypeInfo(nameof(TireEffectRecord))]
public class TireEffectRecord : VltBaseType<Key32>
{
    public TireCondition mTireCondition { get; set; }
    public RefSpecPacked32 mEmitter { get; set; } = new();
    public float mMinSpeed { get; set; }
    public float mMaxSpeed { get; set; }

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        mEmitter.Read(context, fieldContext, br);
        mTireCondition = br.ReadEnum<TireCondition>();
        mMinSpeed = br.ReadSingle();
        mMaxSpeed = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        mEmitter.Write(context, fieldContext, bw);
        bw.WriteEnum(mTireCondition);
        bw.Write(mMinSpeed);
        bw.Write(mMaxSpeed);
    }

    public override object Clone()
    {
        return new TireEffectRecord
        {
            mTireCondition = mTireCondition,
            mEmitter = (RefSpecPacked32)mEmitter.Clone(),
            mMinSpeed = mMinSpeed,
            mMaxSpeed = mMaxSpeed
        };
    }
}