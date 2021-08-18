using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DoubTech.ScriptableEvents.BuiltinTypes;

namespace DoubTech.OpenPath.Data.Equipment
{
    /// <summary>
    /// Ship to Ship Lasers can be equipped to many ships and are used for attack and defense in
    /// ship to ship combat. They are not effective against ground targets.
    /// </summary>
    [CreateAssetMenu(fileName = "Laser - Ship to Ship", menuName = "OpenPath/Config/Laser - Ship to Ship")]
    public class ShipToShipLaser : AbstractShipWeapon
    {
        internal override bool CanDamage(Transform target)
        {
            return target.GetComponent<ShipDamageController>() != null;
        }
    }
}
