using LethalCompanyInputUtils.Api;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;

namespace ReservedPeeperSlot.Input
{
    internal class IngameKeybinds : LcInputActions
    {
        public static IngameKeybinds Instance = new();

        internal static InputActionAsset GetAsset()
        {
            return Instance.Asset;
        }

        [InputAction("<Keyboard>/g", Name = "[ReservedPeeperSlot]\nDrop Peeper")]
        public InputAction DropPeeperAction { get; set; }
    }
}
