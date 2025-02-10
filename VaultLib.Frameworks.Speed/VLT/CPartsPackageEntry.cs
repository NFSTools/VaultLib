using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CPartsPackageEntry))]
public class CPartsPackageEntry : VltBaseType<Key32>
{
    public RefSpec32 Part { get; set; } = new();
    public byte KitNum { get; set; }

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        Part.Read(context, fieldContext, br);
        KitNum = br.ReadByte();
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        Part.Write(context, fieldContext, bw);
        bw.Write(KitNum);
        bw.AlignWriter(4);
    }

    public override object Clone()
    {
        return new CPartsPackageEntry
        {
            Part = (RefSpec32)this.Part.Clone(),
            KitNum = this.KitNum,
        };
    }
}