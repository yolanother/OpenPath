using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Data.Equipment;
using System.Text;
using System;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Weapon Controller handles targetting and firing of the ships weapons.
    /// </summary>
    public class ShipWeaponController : AbstractActionController
    {
        [SerializeField, Tooltip("The weapon this controller is in charge of.")]
        internal AbstractShipWeapon weapon;

        public override string StatusAsString()
        {
            if (weapon == null)
            {
                return "No weapons equipped";
            } else
            {
                return weapon.name;
            }
        }

        internal bool Equip(AbstractShipWeapon equipment)
        {
            weapon = equipment;
            return true;
        }
    }
}
