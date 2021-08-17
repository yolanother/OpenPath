using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DoubTech.OpenPath.Data.Equipment
{
    public abstract class AbstractShipWeapon : AbstractShipEquipment
    {
        [SerializeField, Tooltip("Base damage done by this weapon under normal circumstances.")]
        internal float baseDamage = 10;

        /// <summary>
        /// Fire the lasers at a target.
        /// </summary>
        /// <param name="target">The target to fire upon.</param>
        internal abstract void Fire(Transform target);
    }
}
