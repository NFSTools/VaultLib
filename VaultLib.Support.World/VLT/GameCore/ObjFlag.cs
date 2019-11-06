using System;

namespace VaultLib.Support.World.VLT.GameCore
{
    [Flags]
    public enum ObjFlag
    {
        kFlag_None = 0,
        kFlag_Engagable = 64,
        kFlag_Activatable = 128,
    }
}
