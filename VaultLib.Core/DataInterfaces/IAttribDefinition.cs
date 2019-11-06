using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.DataInterfaces
{
    public interface IAttribDefinition : IFileAccess
    {
        ulong Key { get; set; }
        ulong Type { get; set; }
        ushort Offset { get; set; }
        ushort Size { get; set; }
        ushort MaxCount { get; set; }
        DefinitionFlags Flags { get; set; }
        int Alignment { get; set; }
    }
}