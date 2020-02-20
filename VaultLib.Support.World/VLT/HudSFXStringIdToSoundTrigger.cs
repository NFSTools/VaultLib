using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(HudSFXStringIdToSoundTrigger))]
    public class HudSFXStringIdToSoundTrigger : VLTBaseType
    {
        public HudSFXStringIdToSoundTrigger(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public HudSFXStringIdToSoundTrigger(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint Value1 { get; set; }
        public uint Value2 { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Value1 = br.ReadUInt32();
            Value2 = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Value1);
            bw.Write(Value2);
        }
    }
}