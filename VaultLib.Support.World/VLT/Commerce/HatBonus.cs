// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 9:26 AM.

using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Commerce
{
    [VltTypeInfo("Commerce::HatBonus")]
    public struct HatBonus
    {
        public int Handling;
        public int Acceleration;
        public int TopSpeed;
        public int RequiredPartCount;
    }
}