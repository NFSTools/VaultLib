using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.Hub
{
    [VLTTypeInfo("Hub::HubType")]
    public enum HubType
    {
        kType_Preset = 0x2,
        kType_Career = 0x6,
        kType_Custom = 0x8,
    }
}