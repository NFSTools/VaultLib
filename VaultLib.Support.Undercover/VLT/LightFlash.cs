using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    // TODO: figure this out
    [VltTypeInfo(nameof(LightFlash))]
    public class LightFlash : VltBaseType
    {
        public LightFlash(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public LightFlash(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public float Value1 { get; set; }
        public float Value2 { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Value1 = br.ReadSingle();
            Value2 = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(Value1);
            bw.Write(Value2);
        }
    }
}