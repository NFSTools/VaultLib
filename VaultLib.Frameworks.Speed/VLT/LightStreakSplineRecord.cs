using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(LightStreakSplineRecord))]
    public class LightStreakSplineRecord : VltBaseType
    {
        public LightStreakSplineRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public LightStreakSplineRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint mEnum { get; set; }
        public uint mIndex { get; set; }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            mEnum = br.ReadUInt32();
            mIndex = br.ReadUInt32();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            bw.Write(mEnum);
            bw.Write(mIndex);
        }
    }
}