using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Equipment;
using DoubTech.OpenPath.Eqipment;
using System;
using Random = UnityEngine.Random;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Damage Controller manages damage and repairs of something in the universe. Drop this controller onto
    /// any object that can be attacked.
    /// </summary>
    public class ShipDamageController : AbstractActionController, IDamageController
    {
        [SerializeField, Tooltip("The maximum hit points for this object.")]
        float maxHitPoints = 100;
        [SerializeField, Tooltip("The current hit points. If this goes to zero the object is destroyed.")]
        float currentHitPoints = 100;
        [SerializeField, Tooltip("Shields equipped here will protect the ship from damage, but once they are depleted the ship will fall quickly.")]
        ShipShieldEquipment shields;
        [SerializeField, Tooltip("Prefab to spawn when recieving damage.")]
        GameObject damageFeedbackPrefab;

        public float PercentHitPoints => currentHitPoints / maxHitPoints;

        private void Update()
        {
            if (shields != null && shields.HitPoints < shields.maxHitPoints)
            {
                shields.Regenerate(Time.deltaTime);
            }
        }

        internal bool Equip(ShipShieldEquipment equipment)
        {
            shields = equipment;
            return true;
        }

        /// <summary>
        /// Add damage to this object.
        /// </summary>
        /// <param name="attacker">The source of the damage.</param>
        /// <param name="damageAmount">The amount of the damage.</param>
        public void AddDamage(AbstractShipWeapon weapon, float damageAmount)
        {
            StartCoroutine(DamageFeedbackCo(weapon, damageAmount));

            float hullDamage = damageAmount;
            if (shields != null && shields.HitPoints > 0)
            {
                if (shields.HitPoints > damageAmount)
                {
                    shields.HitPoints -= damageAmount;
                    Debug.Log($"{gameObject.name}'s shields absorbed {damageAmount} of damage from {weapon.owner}.");
                    return;
                } else
                {
                    Debug.Log($"{gameObject.name}'s shields absorbed {shields.HitPoints} of damage from {weapon.owner}, but are now depleted.");
                    hullDamage = damageAmount - shields.HitPoints;
                }
            }

            currentHitPoints -= hullDamage;
            Debug.Log($"{weapon.owner.name} attacked {gameObject.name} with {weapon.name} for {hullDamage} damage. Current Hit Points: {currentHitPoints}");

            shipController.WeaponController.onAlert = true;
            shipController.WeaponController.SetTarget(weapon.owner);

            if (currentHitPoints <= 0) Die();
        }

        private IEnumerator DamageFeedbackCo(AbstractShipWeapon weapon, float damageAmount)
        {
            yield return null;

            Vector3 offset = Random.insideUnitSphere * 0.2f;
            //TODO use pooling for explosions
            Destroy(Instantiate(damageFeedbackPrefab, transform.position + offset, Quaternion.identity, transform), 5);
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
