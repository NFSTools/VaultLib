// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:56 PM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(GMapCurveRef))]
    public struct GMapCurveRef
    {
        public enum GMapCurveRefFlags : ushort
        {
            kFlag_Reversed = 1,
        }

        public ushort mCurveIndex;
        public GMapCurveRefFlags Flags;
    }
}