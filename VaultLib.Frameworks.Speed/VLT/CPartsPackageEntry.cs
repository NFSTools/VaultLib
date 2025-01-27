using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CPartsPackageEntry))]
public class CPartsPackageEntry : VltBaseType<uint>
{
    public RefSpec<uint> Part { get; set; } = new();
    public byte KitNum { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Part.Read(context, fieldContext, br);
        KitNum = br.ReadByte();
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        Part.Write(context, fieldContext, bw);
        bw.Write(KitNum);
        bw.AlignWriter(4);
    }
}