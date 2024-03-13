using BepInEx.Configuration;
using System.Collections.Generic;

namespace ReservedPeeperSlot
{
    public static class Settings
    {
        public static Dictionary<string, ConfigEntryBase> ConfigEntries = [];
        public static ConfigEntry<string> DeployPeeperKey;
        public static ConfigEntry<bool> DeployPeeper;
        public static ConfigEntry<int> PeeperPrice;
        public static void Init()
        {
            DeployPeeperKey = Plugin.Instance.Config.Bind("ReservedPeeperSlot", "DropPeeperKey", "<Keyboard>/t", "Keybind to drop the Peeper on the ground. (will be ignored if InputUtils is present)");
            DeployPeeper = Plugin.Instance.Config.Bind("ReservedPeeperSlot", "DeployPeeper", true, "Enable the keybind drop the Peeper.");
            PeeperPrice = Plugin.Instance.Config.Bind("ReservedPeeperSlot", "PeeperPrice", 0, "Price for the Peeper Slot. If set to 0 the slot will unlock automatically");

            ConfigEntries.Add(DeployPeeperKey.Definition.Key, DeployPeeperKey);
            ConfigEntries.Add(DeployPeeper.Definition.Key, DeployPeeper);
            ConfigEntries.Add(PeeperPrice.Definition.Key, PeeperPrice);
        }
    }
}
