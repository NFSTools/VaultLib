using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(LightStreakSplineRecord))]
    public class LightStreakSplineRecord : VltBaseType
    {
        public uint mEnum { get; set; }
        public uint mIndex { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            mEnum = br.ReadUInt32();
            mIndex = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(mEnum);
            bw.Write(mIndex);
        }
    }
}