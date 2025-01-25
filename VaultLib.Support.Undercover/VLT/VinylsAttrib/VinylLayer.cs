using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

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
        PartNameHash = br.ReadUInt32();
        Mirrored = br.ReadBoolean();
        br.AlignReader(2);
        Transform.Read(context, fieldContext, br);
        br.AlignReader(2);
        foreach (var t in Colors)
        {
            t.Read(context, fieldContext, br);
        }
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        bw.Write(PartNameHash);
        bw.Write(Mirrored);
        bw.AlignWriter(2);
        Transform.Write(context, fieldContext, bw);
        bw.AlignWriter(2);
        for (var i = 0; i < 4; i++)
        {
            Colors[i].Write(context, fieldContext, bw);
        }
    }
}