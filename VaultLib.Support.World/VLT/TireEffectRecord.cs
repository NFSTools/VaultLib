using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(TireEffectRecord))]
    public class TireEffectRecord : VLTBaseType
    {
        public TireEffectRecord(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            mEmitter = new RefSpecPacked(Class, Field, Collection);
        }

        public TireCondition mTireCondition { get; set; }
        public RefSpecPacked mEmitter { get; set; }
        public float mMinSpeed { get; set; }
        public float mMaxSpeed { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            mTireCondition = br.ReadEnum<TireCondition>();
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