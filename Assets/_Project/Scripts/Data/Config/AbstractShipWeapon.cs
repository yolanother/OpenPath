using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DoubTech.OpenPath.Data.Equipment
{
    public abstract class AbstractShipWeapon : AbstractShipEquipment
    {
        [SerializeField, Tooltip("Base damage done by this weapon under normal circumstances.")]
        internal float baseDamage = 10;
        [SerializeField, Tooltip("The time a weapon must cooldown before it can fire again")]
        float cooldown = 1f;

        float timeCooldownOver;
        internal Transform currentTarget;

        internal bool OnCooldown { get => Time.timeSinceLevelLoad <= timeCooldownOver; }

        /// <summary>
        /// Check to see if the weapon can fire and, if it can do so.
        /// </summary>
        /// <param name="target">The target to fire upon.</param>
        internal void Fire(Transform target)
        {
            if (OnCooldown) return;

            currentTarget = target;
            PullTrigger();
            timeCooldownOver = Time.timeSinceLevelLoad + cooldown;
        }

        internal abstract void PullTrigger();
    }
}
