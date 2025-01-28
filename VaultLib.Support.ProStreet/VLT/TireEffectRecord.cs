using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(TireEffectRecord))]
public class TireEffectRecord: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public TireCondition mTireCondition { get; set; }
    public RefSpec32 mEmitter { get; set; } = new();
    public RefSpec32 mEmitterLowLod { get; set; } = new();
    public float mMinSpeed { get; set; }
    public float mMaxSpeed { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        mTireCondition = br.ReadEnum<TireCondition>();
        mEmitter.Read(context, fieldContext, br);
        mEmitterLowLod.Read(context, fieldContext, br);
        mMinSpeed = br.ReadSingle();
        mMaxSpeed = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(mTireCondition);
        mEmitter.Write(context, fieldContext, bw);
        mEmitterLowLod.Write(context, fieldContext, bw);
        bw.Write(mMinSpeed);
        bw.Write(mMaxSpeed);
    }
}