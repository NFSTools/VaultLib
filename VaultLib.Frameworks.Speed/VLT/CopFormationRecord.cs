using System.Buffers.Binary;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CopFormationRecord))]
public struct CopFormationRecord : IComplexType
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

    public FormationTypeEnum FormationType;
    public float Duration;
    public float Frequency;

    public void EndianSwap()
    {
        FormationType = (FormationTypeEnum)BinaryPrimitives.ReverseEndianness((uint)FormationType);
        Duration = Duration.EndianSwap();
        Frequency = Frequency.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}