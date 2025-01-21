using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib;

[VltTypeInfo("VinylsAttrib::VinylLayer")]
public class VinylLayer : VltBaseType
{
    public VinylLayer()
    {
        Transform = new VinylTransform();
        Colors = new VinylColor[4];
        for (int i = 0; i < 4; i++)
        {
            Colors[i] = new VinylColor();
        }
    }

    public uint PartNameHash { get; set; }
    public bool Mirrored { get; set; }
    public VinylTransform Transform { get; set; }
    public VinylColor[] Colors { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        PartNameHash = br.ReadUInt32(); // 4
        Mirrored = br.ReadBoolean(); // 5
        br.AlignReader(4); // 5 + (4 - 5 % 4) = 8
        Transform.Read(context, fieldContext, br);
        for (int i = 0; i < 4; i++)
        {
            Colors[i].Read(context, fieldContext, br);
        }
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        bw.Write(PartNameHash);
        bw.Write(Mirrored);
        bw.AlignWriter(4);
        Transform.Write(context, fieldContext, bw);
        for (int i = 0; i < 4; i++)
        {
            Colors[i].Write(context, fieldContext, bw);
        }
    }
}