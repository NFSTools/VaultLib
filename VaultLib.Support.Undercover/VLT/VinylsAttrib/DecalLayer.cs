using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib
{
    [VltTypeInfo("VinylsAttrib::DecalLayer")]
    public class DecalLayer : VltBaseType
    {
        public DecalLayer(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Transform = new VinylTransform(Class, Field, Collection);
            Color = new VinylColor(Class, Field, Collection);
        }

        public uint PartNameHash { get; set; }
        public bool Mirrored { get; set; }
        public VinylTransform Transform { get; set; }
        public VinylColor Color { get; set; }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            PartNameHash = br.ReadUInt32();
            Mirrored = br.ReadBoolean();
            br.AlignReader(4);
            Transform.Read(context, br);
            Color.Read(context, br);
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.Write(PartNameHash);
            bw.Write(Mirrored);
            bw.AlignWriter(4);
            Transform.Write(context, bw);
            Color.Write(context, bw);
        }
    }
}