using System;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [Flags]
    [VLTTypeInfo("GameCore::PoiBehaviorFlags")]
    public enum PoiBehavior
    {
        kPoi_None = 0,
        kPoi_Icon = 1,
    }
}
