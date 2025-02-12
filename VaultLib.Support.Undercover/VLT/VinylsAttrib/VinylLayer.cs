using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib;

[VltTypeInfo("VinylsAttrib::VinylLayer")]
public class VinylLayer : VltBaseType<Core.DataInterfaces.Key32>
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

    public BinKey32 PartNameHash { get; set; }
    public bool Mirrored { get; set; }
    public VinylTransform Transform { get; set; }
    public VinylColor[] Colors { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        PartNameHash = BinKey32.Read(br);
        Mirrored = br.ReadBoolean();
        br.AlignReader(2);
        Transform.Read(context, fieldContext, br);
        br.AlignReader(2);
        foreach (var t in Colors)
        {
            t.Read(context, fieldContext, br);
        }
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        PartNameHash.Write(bw);
        bw.Write(Mirrored);
        bw.AlignWriter(2);
        Transform.Write(context, fieldContext, bw);
        bw.AlignWriter(2);
        for (var i = 0; i < 4; i++)
        {
            Colors[i].Write(context, fieldContext, bw);
        }
    }

    public override object Clone()
    {
        return new VinylLayer
        {
            PartNameHash = PartNameHash,
            Mirrored = Mirrored,
            Transform = (VinylTransform)Transform.Clone(),
            Colors = Colors.CloneComplex()
        };
    }
}