using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(TireEffectRecord))]
    public class TireEffectRecord : VLTBaseType
    {
        public TireEffectRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public TireEffectRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public TireCondition mTireCondition { get; set; }
        public RefSpecPacked mEmitter { get; set; }
        public float mMinSpeed { get; set; }
        public float mMaxSpeed { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            mTireCondition = br.ReadEnum<TireCondition>();
            mEmitter = new RefSpecPacked(Class, Field, Collection);
            mEmitter.Read(vault, br);
            mMinSpeed = br.ReadSingle();
            mMaxSpeed = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(mTireCondition);
            mEmitter.Write(vault, bw);
            bw.Write(mMinSpeed);
            bw.Write(mMaxSpeed);
        }
    }
}