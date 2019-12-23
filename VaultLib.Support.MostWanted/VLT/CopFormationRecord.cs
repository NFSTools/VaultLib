using CoreLibraries.IO;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.MostWanted.VLT
{
    [VLTTypeInfo(nameof(CopFormationRecord))]
    public class CopFormationRecord : VLTBaseType
    {
        public enum FormationTypeEnum
        {
            PIT = 1,
            BOX_IN = 2,
            ROLLING_BLOCK = 3,
            FOLLOW = 4,
            HELI_PURSUIT = 5,
            HERD = 6,
            ROLLING_BLOCK_LARGE = 7,
            CHASE_UP_FRONT = 8,
            CHASE_DIAGONAL = 9,
            CHASE_RIGHT_TRIANGLE = 10,
            CHASE_TRIANGLE = 11,
            CHASE_REVERSE_TRIANGLE = 12,
            REAR_RAM = 13,
            SIDE_RAM = 14,
            FRONT_RAM = 15,
        }

        public FormationTypeEnum FormationType { get; set; }
        public float Duration { get; set; }
        public float Frequency { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            FormationType = br.ReadEnum<FormationTypeEnum>();
            Duration = br.ReadSingle();
            Frequency = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(FormationType);
            bw.Write(Duration);
            bw.Write(Frequency);
        }

        public CopFormationRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CopFormationRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}