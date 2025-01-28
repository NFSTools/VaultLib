using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.DataInterfaces;

public interface IAttribDefinition<TKey> : IVaultFileAccess<TKey> where TKey : struct, IKey<TKey>
{
    TKey Key { get; set; }
    TKey Type { get; set; }
    ushort Offset { get; set; }
    ushort Size { get; set; }
    ushort MaxCount { get; set; }
    DefinitionFlags Flags { get; set; }
    int Alignment { get; set; }
}