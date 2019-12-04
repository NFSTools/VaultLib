using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore.Pursuit
{
    [VLTTypeInfo("GameCore::Pursuit::CopFormationRecord")]
    public class CopFormationRecord : VLTBaseType
    {
        public FormationType FormationType { get; set; }
        public float Duration { get; set; }
        public float Frequency { get; set; }
        
        public override void Read(Vault vault, BinaryReader br)
        {
            FormationType = br.ReadEnum<FormationType>();
            Duration = br.ReadSingle();
            Frequency = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(FormationType);
            bw.Write(Duration);
            bw.Write(Frequency);
        }

        public CopFormationRecord(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public CopFormationRecord(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}