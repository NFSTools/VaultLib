using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib
{
    [VltTypeInfo("VinylsAttrib::DecalLayer")]
    public class DecalLayer : VltBaseType
    {
        public uint PartNameHash { get; set; }
        public bool Mirrored { get; set; }
        public VinylTransform Transform { get; set; } = new();
        public VinylColor Color { get; set; } = new();

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            PartNameHash = br.ReadUInt32();
            Mirrored = br.ReadBoolean();
            br.AlignReader(4);
            Transform.Read(context, fieldContext, br);
            Color.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(PartNameHash);
            bw.Write(Mirrored);
            bw.AlignWriter(4);
            Transform.Write(context, fieldContext, bw);
            Color.Write(context, fieldContext, bw);
        }
    }
}