using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(TireTimeEffectRecord))]
public class TireTimeEffectRecord: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public TireCondition mTireCondition { get; set; }
    public RefSpec<VaultLib.Core.DataInterfaces.Key32> mEmitter { get; set; } = new();
    public RefSpec<VaultLib.Core.DataInterfaces.Key32> mEmitterLowLod { get; set; } = new();
    public float mMinTime { get; set; }
    public float mMaxTime { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        mTireCondition = br.ReadEnum<TireCondition>();
        mEmitter.Read(context, fieldContext, br);
        mEmitterLowLod.Read(context, fieldContext, br);
        mMinTime = br.ReadSingle();
        mMaxTime = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(mTireCondition);
        mEmitter.Write(context, fieldContext, bw);
        mEmitterLowLod.Write(context, fieldContext, bw);
        bw.Write(mMinTime);
        bw.Write(mMaxTime);
    }
}