using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib
{
    [VLTTypeInfo("VinylsAttrib::DecalLayer")]
    public class DecalLayer : VLTBaseType
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

        public override void Read(Vault vault, BinaryReader br)
        {
            PartNameHash = br.ReadUInt32();
            Mirrored = br.ReadBoolean();
            br.AlignReader(4);
            Transform.Read(vault, br);
            Color.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(PartNameHash);
            bw.Write(Mirrored);
            bw.AlignWriter(4);
            Transform.Write(vault, bw);
            Color.Write(vault, bw);
        }
    }
}