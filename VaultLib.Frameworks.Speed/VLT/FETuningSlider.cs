using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FETuningSlider))]
public class FETuningSlider : VltBaseType<Key32>
{
    public RefSpec32 Ref { get; set; } = new();
    public BinKey32 TitleHash { get; set; }
    public BinKey32 LeftHash { get; set; }
    public BinKey32 RightHash { get; set; }
    public BinKey32 HelpHash { get; set; }

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        Ref.Read(context, fieldContext, br);
        TitleHash = BinKey32.Read(br);
        LeftHash = BinKey32.Read(br);
        RightHash = BinKey32.Read(br);
        HelpHash = BinKey32.Read(br);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        Ref.Write(context, fieldContext, bw);
        TitleHash.Write(bw);
        LeftHash.Write(bw);
        RightHash.Write(bw);
        HelpHash.Write(bw);
    }

    public override object Clone()
    {
        return new FETuningSlider
        {
            Ref = (RefSpec32)this.Ref.Clone(),
            TitleHash = this.TitleHash,
            LeftHash = this.LeftHash,
            RightHash = this.RightHash,
            HelpHash = this.HelpHash,
        };
    }
}