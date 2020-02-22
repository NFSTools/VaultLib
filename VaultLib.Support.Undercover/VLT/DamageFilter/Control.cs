using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.DamageFilter
{
    [VLTTypeInfo("DamageFilter::Control")]
    public class Control : VLTBaseType
    {
        public Control(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Control(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint Allow { get; set; }
        public uint Reject { get; set; }
        public float MaxCausalityTime { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Allow = br.ReadUInt32();
            Reject = br.ReadUInt32();
            MaxCausalityTime = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Allow);
            bw.Write(Reject);
            bw.Write(MaxCausalityTime);
        }
    }
}