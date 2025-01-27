using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(TireTimeEffectRecord))]
public class TireTimeEffectRecord: VltBaseType<uint>
{
    public TireCondition mTireCondition { get; set; }
    public RefSpec<uint> mEmitter { get; set; } = new();
    public RefSpec<uint> mEmitterLowLod { get; set; } = new();
    public float mMinTime { get; set; }
    public float mMaxTime { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        mTireCondition = br.ReadEnum<TireCondition>();
        mEmitter.Read(context, fieldContext, br);
        mEmitterLowLod.Read(context, fieldContext, br);
        mMinTime = br.ReadSingle();
        mMaxTime = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(mTireCondition);
        mEmitter.Write(context, fieldContext, bw);
        mEmitterLowLod.Write(context, fieldContext, bw);
        bw.Write(mMinTime);
        bw.Write(mMaxTime);
    }
}