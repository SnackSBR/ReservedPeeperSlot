using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using ReservedItemSlotCore.Patches;
using ReservedItemSlotCore;
using ReservedPeeperSlot.Compat;
using System;

namespace ReservedPeeperSlot.Input
{
    [HarmonyPatch]
    internal static class Keybinds
    {
        public static InputActionAsset Asset;
        public static InputActionMap ActionMap;
        private static InputAction DropPeeperAction;

        public static PlayerControllerB LocalPlayerController => StartOfRound.Instance?.localPlayerController;

        [HarmonyPatch(typeof(PreInitSceneScript), "Awake")]
        [HarmonyPrefix]
        public static void AddToKeybindMenu()
        {
            if(Settings.DeployPeeper.Value)
            {
                if (InputUtils_Compat.Enabled)
                {
                    Asset = InputUtils_Compat.Asset;
                    ActionMap = Asset.actionMaps[0];
                    DropPeeperAction = IngameKeybinds.Instance.DropPeeperAction;
                }
                else
                {
                    Asset = ScriptableObject.CreateInstance<InputActionAsset>();
                    ActionMap = new InputActionMap("ReservedPeeperSlot");
                    InputActionSetupExtensions.AddActionMap(Asset, ActionMap);
                    DropPeeperAction = InputActionSetupExtensions.AddAction(ActionMap, "ReservedPeeperSlot.DropPeeperAction", binding: Settings.DeployPeeperKey.Value, interactions: "Press");
                }
            }
        }

        [HarmonyPatch(typeof(StartOfRound), "OnEnable")]
        [HarmonyPostfix]
        public static void OnEnable()
        {
            if(Settings.DeployPeeper.Value)
            {
                Asset.Enable();
                DropPeeperAction.performed += OnDropRadarBoosterPerformed;
            }
        }

        [HarmonyPatch(typeof(StartOfRound), "OnDisable")]
        [HarmonyPostfix]
        public static void OnDisable()
        {
            if (Settings.DeployPeeper.Value)
            {
                Asset.Disable();
                DropPeeperAction.performed -= OnDropRadarBoosterPerformed;
            }
        }

        private static void OnDropRadarBoosterPerformed(InputAction.CallbackContext context)
        {
            if(LocalPlayerController == null || !LocalPlayerController.isPlayerControlled || (LocalPlayerController.IsServer && !LocalPlayerController.isHostPlayerObject))
            {
                return;
            }

            ReservedItemInfo peeper = Plugin.PeeperInfo;

            ReservedItemSlotCore.Input.Keybinds.holdingModifierKey = true;

            if (ReservedItemPatcher.reservedHotbarSize > 0 && !ReservedItemPatcher.IsItemSlotEmpty(peeper, LocalPlayerController) && ReservedItemPatcher.CanSwapToReservedHotbarSlot())
            {
                try
                {
                    LocalPlayerController.StartCoroutine(ShuffleItems(peeper));
                }
                catch (Exception ex)
                {
                    Plugin.mls.LogError(ex.Message);
                    Plugin.mls.LogError(ex.StackTrace);
                    HUDManager.Instance.chatText.text += $"Error when dropping ${peeper.itemName}";
                }
            }

            ReservedItemSlotCore.Input.Keybinds.holdingModifierKey = false;
            ReservedItemPatcher.FocusReservedHotbarSlots(false);
        }

        private static IEnumerator ShuffleItems(ReservedItemInfo peeper)
        {
            ReservedPlayerData reservedPlayerData = PlayerPatcher.playerDataLocal;
            ReservedItemPatcher.FocusReservedHotbarSlots(true);
            yield return new WaitForSeconds(0.1f);
            ReservedItemPatcher.SwitchToItemSlot(LocalPlayerController, reservedPlayerData.reservedHotbarStartIndex + peeper.reservedItemIndex, null);
            yield return new WaitForSeconds(0.1f);
            LocalPlayerController.DiscardHeldObject();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
