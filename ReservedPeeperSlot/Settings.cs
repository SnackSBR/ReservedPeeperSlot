using BepInEx.Configuration;
using System.Collections.Generic;

namespace ReservedPeeperSlot
{
    public static class Settings
    {
        public static Dictionary<string, ConfigEntryBase> ConfigEntries = [];
        public static ConfigEntry<string> DeployPeeperKey;
        public static ConfigEntry<bool> DeployPeeper;
        public static void Init()
        {
            DeployPeeperKey = Plugin.Instance.Config.Bind("ReservedPeeperSlot", "DropPeeperKey", "<Keyboard>/g", "Keybind to drop the Peeper on the ground. (will be ignored if InputUtils is present)");
            DeployPeeper = Plugin.Instance.Config.Bind("ReservedPeeperSlot", "DeployPeeper", true, "Enable the keybind drop the Peeper.");

            ConfigEntries.Add(DeployPeeperKey.Definition.Key, DeployPeeperKey);
            ConfigEntries.Add(DeployPeeper.Definition.Key, DeployPeeper);
        }
    }
}
