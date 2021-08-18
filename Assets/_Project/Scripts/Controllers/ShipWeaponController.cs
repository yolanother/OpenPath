using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Data.Equipment;
using System.Text;
using System;
using Random = UnityEngine.Random;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Weapon Controller handles targetting and firing of the ships weapons.
    /// </summary>
    public class ShipWeaponController : AbstractActionController
    {
        [SerializeField, Tooltip("The default weapon that comes with this controller as standard.")]
        internal AbstractShipWeapon defaultWeapon;
        [SerializeField, Tooltip("The weapon this controller is in charge of.")]
        internal AbstractShipWeapon weapon;
        [SerializeField, Tooltip("The frequency at which the ship will scan for enemy ships.")]
        float scanFrequency = 2f;

        internal AbstractShipWeapon EquippedWeapon
        {
            get => weapon;
            set
            {
                if (weapon != value)
                {
                    Destroy(weapon);
                    weapon = value;
                }
            }
        }

        ShipController currentTargetShip;
        float timeOfNextScan;

        /// <summary>
        /// If a ship is OnAlert it will actively scan for enemy ships.
        /// If one is found it will be attacked.
        /// </summary>
        public bool OnAlert { get; set; }

        private void Update()
        {
            if (EquippedWeapon == null)
            {
                if (defaultWeapon != null)
                {
                    EquippedWeapon = Instantiate(defaultWeapon);
                } else
                {
                    return;
                }
            }

            if (currentTargetShip != null)
            {
                if (!EquippedWeapon.OnCooldown)
                {
                    EquippedWeapon.Fire(currentTargetShip.transform);
                }
            }
            else if (OnAlert && Time.timeSinceLevelLoad > timeOfNextScan)
            {
                List<ShipController> ships = ScanForObjectsOfType<ShipController>();
                for (int i = 0; i < ships.Count; i++)
                {
                    if (ships[i].faction != shipController.faction)
                    {
                        currentTargetShip = ships[i];
                        break;
                    }
                }
            }
        }

        internal void SetTarget(ShipController target)
        {
            currentTargetShip = target;
        }

        public override string StatusAsString()
        {
            if (EquippedWeapon == null)
            {
                return "No weapons equipped";
            } else
            {
                return EquippedWeapon.name;
            }
        }

        internal bool Equip(AbstractShipWeapon equipment)
        {
            EquippedWeapon = equipment;
            return true;
        }
    }
}
