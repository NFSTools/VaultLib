using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(TireTimeEffectRecord))]
public class TireTimeEffectRecord : VltBaseType<Core.DataInterfaces.Key32>
{
    public TireCondition mTireCondition { get; set; }
    public RefSpec32 mEmitter { get; set; } = new();
    public RefSpec32 mEmitterLowLod { get; set; } = new();
    public float mMinTime { get; set; }
    public float mMaxTime { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        mTireCondition = br.ReadEnum<TireCondition>();
        mEmitter.Read(context, fieldContext, br);
        mEmitterLowLod.Read(context, fieldContext, br);
        mMinTime = br.ReadSingle();
        mMaxTime = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(mTireCondition);
        mEmitter.Write(context, fieldContext, bw);
        mEmitterLowLod.Write(context, fieldContext, bw);
        bw.Write(mMinTime);
        bw.Write(mMaxTime);
    }

    public override object Clone()
    {
        return new TireTimeEffectRecord
        {
            mTireCondition = mTireCondition,
            mEmitter = (RefSpec32)mEmitter.Clone(),
            mEmitterLowLod = (RefSpec32)mEmitterLowLod.Clone(),
            mMinTime = mMinTime,
            mMaxTime = mMaxTime
        };
    }
}