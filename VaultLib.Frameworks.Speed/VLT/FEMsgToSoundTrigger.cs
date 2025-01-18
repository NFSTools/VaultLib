using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(FEMsgToSoundTrigger))]
    public class FEMsgToSoundTrigger : VltBaseType
    {
        public uint FEngMsg { get; set; }
        public eMenuSoundTriggers SoundTrigger { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            FEngMsg = br.ReadUInt32();
            SoundTrigger = br.ReadEnum<eMenuSoundTriggers>();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(FEngMsg);
            bw.WriteEnum(SoundTrigger);
        }
    }
}