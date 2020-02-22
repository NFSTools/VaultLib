using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(FEVinylGroupInfo))]
    public class FEVinylGroupInfo : VLTBaseType
    {
        public FEVinylGroupInfo(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FEVinylGroupInfo(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint Value1 { get; set; }
        public uint Value2 { get; set; }
        public uint Value3 { get; set; }
        public uint Value4 { get; set; }
        public uint Value5 { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Value1 = br.ReadUInt32();
            Value2 = br.ReadUInt32();
            Value3 = br.ReadUInt32();
            Value4 = br.ReadUInt32();
            Value5 = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Value1);
            bw.Write(Value2);
            bw.Write(Value3);
            bw.Write(Value4);
            bw.Write(Value5);
        }
    }
}