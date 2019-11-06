using VaultLib.Core.Utils;

namespace VaultLib.Core.DataInterfaces
{
    public enum EPtrRefType : ushort
    {
        PtrEnd = 0x0,
        PtrNull = 0x1,
        PtrSetFixupTarget = 0x2,
        PtrDepRelative = 0x3,
        PtrExport = 0x4,
    }

    public interface IPtrRef : IFileAccess
    {
        uint FixupOffset { get; set; }
        EPtrRefType PtrType { get; set; }
        ushort Index { get; set; }
        uint Destination { get; set; }
    }
}