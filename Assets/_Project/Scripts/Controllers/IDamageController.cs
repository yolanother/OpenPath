using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Equipment;

namespace DoubTech.OpenPath.Controllers
{
    public interface IDamageController
    {
        /// <summary>
        /// Add damage to this object.
        /// </summary>
        /// <param name="attacker">The source of the damage.</param>
        /// <param name="damageAmount">The amount of the damage.</param>
        public void AddDamage(AbstractShipWeapon weapon, float damageAmount);

        public void Die();
    }
}
