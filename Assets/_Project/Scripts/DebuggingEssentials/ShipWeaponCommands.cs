using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Equipment;
using DebuggingEssentials;
using DoubTech.OpenPath.Controllers;
using Sirenix.OdinInspector;

namespace DoubTech.OpenPath.Debugging
{
    [ConsoleAlias("test.weapon")]
    public class ShipWeaponCommands : AbstractDebugCommands<ShipWeaponController>
    {
        [SerializeField, Tooltip("The target we are testing against.")]
        Transform target;
        [SerializeField, Tooltip("The weapon to test.")]
        AbstractShipWeapon weapon;

        [Button(), HideInEditorMode]
        [ConsoleCommand("buyAndEquipLaser", "Equip a ship to ship laser for all subsequent tests. The ship will go to a planet to purchase one first (it will magically get the credits needed.")]
        public void BuyAndEquipLaser()
        {
            controller.Credits += 10000;
            controller.shipController.TradeController.Buy(weapon, 10000);
        }

        [Button(), HideInEditorMode]
        [ConsoleCommand("fire", "Move to within firing range of the target and fire.")]
        public void Fire()
        {
            controller.weapon.Fire(target);
        }
    }
}
