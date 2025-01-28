using VaultLib.Core.Utils;

namespace VaultLib.Core.DataInterfaces;

public interface IExportEntry<TKey> : IVaultFileAccess<TKey> where TKey : struct, IKey<TKey>
{
    TKey Id { get; set; }
    TKey Type { get; set; }
    uint Size { get; set; }
    uint Offset { get; set; }
}