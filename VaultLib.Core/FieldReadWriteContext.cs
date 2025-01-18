using VaultLib.Core.Data;

namespace VaultLib.Core;

public record FieldReadWriteContext(VltClass Class, VltClassField Field, VltCollection Collection)
{
    public bool IsInVlt => !Field.IsInLayout && Field.Size <= 4 && !Field.IsArray;
}