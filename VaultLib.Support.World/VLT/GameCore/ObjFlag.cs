using System;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [Flags]
    [VLTTypeInfo("GameCore::ObjFlags")]
    public enum ObjFlag
    {
        kFlag_None = 0,
        kFlag_Engagable = 64,
        kFlag_Activatable = 128,
    }
}
