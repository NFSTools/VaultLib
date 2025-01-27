using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore;

[VltTypeInfo("GameCore::EnhancerPart")]
public class EnhancerPart: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public uint Hash { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Hash = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Hash);
    }
}