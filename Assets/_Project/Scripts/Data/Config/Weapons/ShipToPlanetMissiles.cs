using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.SolarSystemScope;

namespace DoubTech.OpenPath.Data.Equipment
{
    /// <summary>
    /// Ship to Ship Lasers can be equipped to many ships and are used for attack and defense in
    /// ship to ship combat. They are not effective against ground targets.
    /// </summary>
    [CreateAssetMenu(fileName = "Missiles - Ship to Planet", menuName = "OpenPath/Config/Missiles - Ship to Planet")]
    public class ShipToPlanetMissiles : AbstractShipWeapon
    {
        internal override bool CanDamage(Transform target)
        {
            return target.GetComponent<PlanetInstance>() != null;
        }
    }
}
