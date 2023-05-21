using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(FEMsgToSoundTrigger))]
    public class FEMsgToSoundTrigger : VLTBaseType
    {
        public FEMsgToSoundTrigger(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FEMsgToSoundTrigger(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint FEngMsg { get; set; }
        public eMenuSoundTriggers SoundTrigger { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            FEngMsg = br.ReadUInt32();
            SoundTrigger = br.ReadEnum<eMenuSoundTriggers>();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(FEngMsg);
            bw.WriteEnum(SoundTrigger);
        }
    }
}