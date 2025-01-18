using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.DamageFilter
{
    [VltTypeInfo("DamageFilter::Control")]
    public class Control : VltBaseType
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

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            Allow = br.ReadUInt32();
            Reject = br.ReadUInt32();
            MaxCausalityTime = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.Write(Allow);
            bw.Write(Reject);
            bw.Write(MaxCausalityTime);
        }
    }
}