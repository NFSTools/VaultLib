using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.World.VLT;

[VltTypeInfo(nameof(TireTimeEffectRecord))]
public class TireTimeEffectRecord : VltBaseType
{
    public TireCondition mTireCondition { get; set; }
    public RefSpec mEmitter { get; set; } = new();
    public RefSpec mEmitterLowLod { get; set; } = new();
    public float mMinTime { get; set; }
    public float mMaxTime { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        mTireCondition = br.ReadEnum<TireCondition>();
        mEmitter.Read(context, fieldContext, br);
        mEmitterLowLod.Read(context, fieldContext, br);
        mMinTime = br.ReadSingle();
        mMaxTime = br.ReadSingle();
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(mTireCondition);
        mEmitter.Write(context, fieldContext, bw);
        mEmitterLowLod.Write(context, fieldContext, bw);
        bw.Write(mMinTime);
        bw.Write(mMaxTime);
    }
}