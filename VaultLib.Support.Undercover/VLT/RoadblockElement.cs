using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

public class RoadblockElement: VltBaseType<uint>
{
    public RBElementType ElementType { get; set; }
    public float OffsetX { get; set; }
    public float OffsetY { get; set; }
    public float Angle { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        ElementType = br.ReadEnum<RBElementType>();
        OffsetX = br.ReadSingle();
        OffsetY = br.ReadSingle();
        Angle = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(ElementType);
        bw.Write(OffsetX);
        bw.Write(OffsetY);
        bw.Write(Angle);
    }
}