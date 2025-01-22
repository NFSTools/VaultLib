using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CollisionStream))]
public class CollisionStream : VltBaseType
{
    public RefSpec StreamMoment { get; set; } = new();
    public byte Threshold { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        StreamMoment.Read(context, fieldContext, br);
        Threshold = br.ReadByte();
        br.AlignReader(4);
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        StreamMoment.Write(context, fieldContext, bw);
        bw.Write(Threshold);
        bw.AlignWriter(4);
    }
}