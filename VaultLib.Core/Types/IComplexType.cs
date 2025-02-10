using VaultLib.Core.Types.Attrib;

namespace VaultLib.Core.Types;

public interface IComplexType
{
    void EndianSwap();
    
    object Clone();
}