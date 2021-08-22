using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Data.Equipment;
using System.Text;
using System;
using Random = UnityEngine.Random;
using DoubTech.OpenPath.Data.UniverseScope;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Weapon Controller handles targetting and firing of the ships weapons.
    /// </summary
    public class ShipWeaponController : AbstractActionController
    {
        [SerializeField, Tooltip("The default weapon that comes with this controller as standard.")]
        internal AbstractShipWeapon defaultWeapon;
        [SerializeField, Tooltip("The weapon this controller is in charge of.")]
        internal AbstractShipWeapon weapon;
        [SerializeField, Tooltip("The frequency at which the ship will scan for enemy ships.")]
        float scanFrequency = 2f;

        internal AbstractShipWeapon equippedWeapon
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
        public bool onAlert { get; set; }

        private void Update()
        {
            if (equippedWeapon == null)
            {
                if (defaultWeapon != null)
                {
                    Equip(Instantiate(defaultWeapon));
                } else
                {
                    return;
                }
            }

            if (currentTargetShip != null)
            {
                //OPTIMIZATION use sqrMagnitude rather than Distance
                if (Vector3.Distance(transform.position, currentTargetShip.transform.position) < equippedWeapon.maxRange)
                {
                    if (!equippedWeapon.OnCooldown)
                    {
                        equippedWeapon.Fire(currentTargetShip.transform);
                    }
                } else
                {
                    if (isAI)
                    {
                        shipController.MovementController.MoveTo(currentTargetShip.transform.position);
                    }
                }
            }
            else if (((isAI && onAlert ) || !GameManager.Instance.areWeaponsPlayerControlled) 
                && Time.timeSinceLevelLoad > timeOfNextScan)
            {
                List<ShipController> ships = ScanForObjectsOfType<ShipController>();
                for (int i = 0; i < ships.Count; i++)
                {
                    if (ships[i].faction != shipController.faction)
                    {
                        if (Random.value <= shipController.aggression)
                        {
                            currentTargetShip = ships[i];
                            break;
                        }
                    }
                }
                timeOfNextScan = Time.timeSinceLevelLoad + scanFrequency;
            }
        }

        internal void SetTarget(ShipController target)
        {
            currentTargetShip = target;
        }

        public override string StatusAsString()
        {
            if (equippedWeapon == null)
            {
                return "No weapons equipped";
            } else
            {
                return equippedWeapon.name;
            }
        }

        internal bool Equip(AbstractShipWeapon weapon)
        {
            equippedWeapon = weapon;
            weapon.owner = shipController;
            return true;
        }
    }
}
