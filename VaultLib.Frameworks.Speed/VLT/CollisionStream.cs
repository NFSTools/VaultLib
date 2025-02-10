using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CollisionStream))]
public class CollisionStream : VltBaseType<Key32>
{
    public RefSpec32 StreamMoment { get; set; } = new();
    public byte Threshold { get; set; }

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        StreamMoment.Read(context, fieldContext, br);
        Threshold = br.ReadByte();
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        StreamMoment.Write(context, fieldContext, bw);
        bw.Write(Threshold);
        bw.AlignWriter(4);
    }

    public override object Clone()
    {
        return new CollisionStream
        {
            StreamMoment = (RefSpec32)StreamMoment.Clone(),
            Threshold = Threshold
        };
    }
}