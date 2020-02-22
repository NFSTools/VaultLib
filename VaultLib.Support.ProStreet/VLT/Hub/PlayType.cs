using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.Hub
{
    [VLTTypeInfo("Hub::PlayType")]
    public enum PlayType
    {
        kPlay_None = 0x0,
        kPlay_Single = 0x1,
        kPlay_MultiLocal = 0x2,
        kPlay_MultiOnline = 0x3,
    }
}