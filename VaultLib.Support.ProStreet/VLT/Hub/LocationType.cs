using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.Hub
{
    [VLTTypeInfo("Hub::LocationType")]
    public enum LocationType
    {
        kLocation_Street = 0x1,
        kLocation_Track = 0x2,
    }
}