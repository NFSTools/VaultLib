using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(LightStreakSplineRecord))]
    public class LightStreakSplineRecord : VLTBaseType
    {
        public LightStreakSplineRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public LightStreakSplineRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint mEnum { get; set; }
        public uint mIndex { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            mEnum = br.ReadUInt32();
            mIndex = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(mEnum);
            bw.Write(mIndex);
        }
    }
}