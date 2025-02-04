using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(ParticleTextureRecord))]
public struct ParticleTextureRecord
{
    public BinKey32 mEnum;
    public uint mIndex;
}