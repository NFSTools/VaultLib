using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore;

[VltTypeInfo("GameCore::PowerupConfiguration")]
public enum PowerupConfiguration
{
    kPowerupConfigFreeRoam,
    kPowerupConfigCircuit,
    kPowerupConfigSprint,
    kPowerupConfigPursuit,
    kPowerupConfigMultiplayerPursuit,
}