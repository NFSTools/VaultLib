using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    public class RoadblockElement : VLTBaseType
    {
        public RoadblockElement(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public RoadblockElement(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public RBElementType ElementType { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public float Angle { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            ElementType = br.ReadEnum<RBElementType>();
            OffsetX = br.ReadSingle();
            OffsetY = br.ReadSingle();
            Angle = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(ElementType);
            bw.Write(OffsetX);
            bw.Write(OffsetY);
            bw.Write(Angle);
        }
    }
}