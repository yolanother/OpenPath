using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;

namespace DoubTech.OpenPath.Data.Equipment
{
    /// <summary>
    /// Ship to Ship Lasers can be equipped to many ships and are used for attack and defense in
    /// ship to ship combat. They are not effective against ground targets.
    /// </summary>
    [CreateAssetMenu(fileName = "Laser - Ship to Ship", menuName = "OpenPath/Config/Laser - Ship to Ship")]
    public class ShipToShipLaser : AbstractShipWeapon
    {
        internal override void PullTrigger()
        {
            Debug.LogFormat("{0} is firing upon {1} with {2}", owner.name, currentTarget.gameObject.name, name);

            DamageController dc = currentTarget.GetComponent<DamageController>();
            if (dc == null) return;

            dc.AddDamage(this, baseDamage);
        }
    }
}
