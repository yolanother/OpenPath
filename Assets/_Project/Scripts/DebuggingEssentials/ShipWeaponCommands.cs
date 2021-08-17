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
        [SerializeField, Tooltip("The ship target we are testing against.")]
        Transform shipTarget;
        [SerializeField, Tooltip("The planet target we are testing against.")]
        Transform planetTarget;
        [SerializeField, Tooltip("The laser weapon to test.")]
        AbstractShipWeapon laserWeapon;
        [SerializeField, Tooltip("The missile weapon to test.")]
        AbstractShipWeapon missileWeapon;

        [Button(), HideInEditorMode]
        [ConsoleCommand("buyAndEquipLaser", "Equip a ship to ship laser for all subsequent tests. The ship will go to a planet to purchase one first (it will magically get the credits needed.")]
        public void BuyAndEquipLaser()
        {
            controller.Credits += 10000;
            controller.shipController.TradeController.Buy(laserWeapon, 10000);
        }

        [Button(), HideInEditorMode]
        [ConsoleCommand("fireOnShip", "Move to within firing range of the target and fire.")]
        public void FireOnShip()
        {
            controller.weapon.Fire(shipTarget);
        }

        [Button(), HideInEditorMode]
        [ConsoleCommand("buyAndEquipMissile", "Equip a ship to planet missile launcher for all subsequent tests. The ship will go to a planet to purchase one first (it will magically get the credits needed.")]
        public void BuyAndEquipMissiles()
        {
            controller.Credits += 100000;
            controller.shipController.TradeController.Buy(missileWeapon, 100000);
        }

        [Button(), HideInEditorMode]
        [ConsoleCommand("fireOnPlanet", "Move to within firing range of the target and fire.")]
        public void FireOnPlanet()
        {
            controller.weapon.Fire(planetTarget);
        }
    }
}
