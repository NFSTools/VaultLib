namespace VaultLib.Core.Types.Attrib.Types;

[VltTypeInfo("Attrib::Types::Colour")]
public struct Colour : IComplexType
{
    public byte A, B, G, R;

    public void EndianSwap()
    {
        (A, B, G, R) = (R, G, B, A);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}