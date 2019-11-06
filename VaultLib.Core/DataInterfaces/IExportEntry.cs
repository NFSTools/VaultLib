using VaultLib.Core.Utils;

namespace VaultLib.Core.DataInterfaces
{
    public interface IExportEntry : IFileAccess
    {
        ulong ID { get; set; }
        ulong Type { get; set; }
        uint Size { get; set; }
        uint Offset { get; set; }
    }
}