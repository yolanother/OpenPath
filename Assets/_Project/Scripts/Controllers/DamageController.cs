using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        public void AddDamage(float damageAmount)
        {
            currentHitPoints -= damageAmount;
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
