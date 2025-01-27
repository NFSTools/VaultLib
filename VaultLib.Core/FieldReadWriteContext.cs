using VaultLib.Core.Data;

namespace VaultLib.Core;

public record FieldReadWriteContext<TKey>(VltClass<TKey> Class, VltClassField<TKey> Field, VltCollection<TKey> Collection)
{
    public bool IsInVlt => !Field.IsInLayout && Field.Size <= 4 && !Field.IsArray;
}