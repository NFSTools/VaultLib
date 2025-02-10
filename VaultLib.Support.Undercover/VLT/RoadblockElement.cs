using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

public class RoadblockElement : VltBaseType<Core.DataInterfaces.Key32>
{
    public RBElementType ElementType { get; set; }
    public float OffsetX { get; set; }
    public float OffsetY { get; set; }
    public float Angle { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        ElementType = br.ReadEnum<RBElementType>();
        OffsetX = br.ReadSingle();
        OffsetY = br.ReadSingle();
        Angle = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(ElementType);
        bw.Write(OffsetX);
        bw.Write(OffsetY);
        bw.Write(Angle);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}