using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.Carbon.VLT;

[VltTypeInfo(nameof(TireEffectRecord))]
public class TireEffectRecord : VltBaseType<Key32>
{
    public RefSpec32 mEmitter { get; set; } = new();
    public float mMinSpeed { get; set; }
    public float mMaxSpeed { get; set; }

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        mEmitter.Read(context, fieldContext, br);
        mMinSpeed = br.ReadSingle();
        mMaxSpeed = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        mEmitter.Write(context, fieldContext, bw);
        bw.Write(mMinSpeed);
        bw.Write(mMaxSpeed);
    }

    public override object Clone()
    {
        return new TireEffectRecord
        {
            mEmitter = (RefSpec32)mEmitter.Clone(),
            mMinSpeed = mMinSpeed,
            mMaxSpeed = mMaxSpeed,
        };
    }
}