using ReservedPeeperSlot.Input;
using UnityEngine.InputSystem;

namespace ReservedPeeperSlot.Compat
{
    public static class InputUtils_Compat
    {
        internal static bool Enabled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.LethalCompanyInputUtils");
        internal static InputActionAsset Asset => IngameKeybinds.GetAsset();
        public static InputAction DropPeeperAction => IngameKeybinds.Instance.DropPeeperAction;

        internal static void Init()
        {
            if (Enabled && IngameKeybinds.Instance == null)
            {
                IngameKeybinds.Instance = new();
            }
        }
    }
}
