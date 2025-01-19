// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 4:01 PM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(GMapTriangle))]
    public struct GMapTriangle
    {
        public ushort mPoint1;
        public ushort mPoint2;
        public ushort mPoint3;
    }
}