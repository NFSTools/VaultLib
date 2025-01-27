using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CollisionStream))]
public class CollisionStream : VltBaseType<uint>
{
    public RefSpec<uint> StreamMoment { get; set; } = new();
    public byte Threshold { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        StreamMoment.Read(context, fieldContext, br);
        Threshold = br.ReadByte();
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext,
        BinaryWriter bw)
    {
        StreamMoment.Write(context, fieldContext, bw);
        bw.Write(Threshold);
        bw.AlignWriter(4);
    }
}