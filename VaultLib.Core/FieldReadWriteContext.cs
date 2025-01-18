using VaultLib.Core.Data;

namespace VaultLib.Core;

public record FieldReadWriteContext(VltClass Class, VltClassField Field, VltCollection Collection);