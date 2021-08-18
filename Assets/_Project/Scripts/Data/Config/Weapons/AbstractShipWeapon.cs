using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DoubTech.OpenPath.Controllers;
using DoubTech.ScriptableEvents.BuiltinTypes;

namespace DoubTech.OpenPath.Data.Equipment
{
    public abstract class AbstractShipWeapon : AbstractShipEquipment
    {
        [SerializeField, Tooltip("Base damage done by this weapon under normal circumstances. For Ship to Ship weapons this will normally be below 100 and will represent the number of hit points of damage done. For ship to planet weapons this will typically map to a percentage of the population killed with each slot, where 100 is around 1%.")]
        internal float baseDamage = 10;
        [SerializeField, Tooltip("The time a weapon must cooldown before it can fire again")]
        float cooldown = 1f;
        [SerializeField] public IntGameEvent fireWeaponEvent;
        [SerializeField] public int weaponEffectIndex;

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

            if (!CanDamage(target))
            {
                Debug.Log($"{owner?.name} attempted to fire on {target?.gameObject?.name} with {name} which can't damage it.");
                return;
            }

            currentTarget = target;
            PullTrigger();
            timeCooldownOver = Time.timeSinceLevelLoad + cooldown;
        }

        /// <summary>
        /// Pulls the trigger of the weapon to finalize the process of firing on the current target.
        /// </summary>
        internal virtual void PullTrigger()
        {
            Debug.LogFormat("{0} is firing upon {1} with {2}", owner?.name, currentTarget?.gameObject?.name, name);

            IDamageController dc = currentTarget.GetComponent<IDamageController>();
            if (dc == null) return;

            dc.AddDamage(this, baseDamage);
        }

        /// <summary>
        /// Return true if this weapon can damage the target provided.
        /// </summary>
        /// <param name="target">The target that we want to fire upon.</param>
        /// <returns>True or false depending on whether this weapon can damage the target.</returns>
        internal abstract bool CanDamage(Transform target);
    }
}
