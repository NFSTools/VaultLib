using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core;

public record FieldReadWriteContext<TKey>(
    VltClass<TKey> Class,
    VltClassField<TKey> Field,
    VltCollection<TKey>? Collection) where TKey : struct, IKey<TKey>
{
    public bool IsInVlt => !Field.IsInLayout && Field.Size <= 4 && !Field.IsArray;
}