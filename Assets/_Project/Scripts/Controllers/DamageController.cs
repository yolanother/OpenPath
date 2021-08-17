using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Equipment;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Damage Controller manages damage and repairs of something in the universe. Drop this controller onto
    /// any object that can be attacked.
    /// </summary>
    public class DamageController : AbstractActionController
    {
        [SerializeField, Tooltip("The maximum hit points for this object.")]
        float maxHitPoints = 100;
        [SerializeField, Tooltip("The current hit points. If this goes to zero the object is destroyed.")]
        float currentHitPoints;

        internal override void Start()
        {
            base.Start();

            if (currentHitPoints == 0)
            {
                currentHitPoints = maxHitPoints;
            }
        }

        /// <summary>
        /// Add damage to this object.
        /// </summary>
        /// <param name="attacker">The source of the damage.</param>
        /// <param name="damageAmount">The amount of the damage.</param>
        public void AddDamage(AbstractShipWeapon weapon, float damageAmount)
        {
            currentHitPoints -= damageAmount;
            Debug.Log($"{weapon.owner.name} attacked {gameObject.name} with {weapon.name} for {damageAmount} damage. Current Hit Points: {currentHitPoints}");

            shipController.WeaponController.OnAlert = true;
            shipController.WeaponController.SetTarget(weapon.owner);

            if (currentHitPoints <= 0) Die();
        }

        public void Die()
        {
            Debug.LogFormat("{0} went KABOOOM!!!", gameObject.name);
            Destroy(gameObject);
        }

        public override string StatusAsString()
        {
            throw new System.NotImplementedException();
        }
    }
}
