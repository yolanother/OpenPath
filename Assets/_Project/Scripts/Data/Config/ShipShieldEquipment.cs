using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Equipment;
using System;

namespace DoubTech.OpenPath.Eqipment
{
    [CreateAssetMenu(fileName = "Shield Configuration", menuName = "OpenPath/Config/Shield")]
    public class ShipShieldEquipment : AbstractShipEquipment
    {
        [SerializeField, Tooltip("The number of hit points this shield will absorb. If this goas to zero the shields are ineffective.")]
        internal float maxHitPoints = 100;
        [SerializeField, Tooltip("The current number of hit points in the shields.")]
        float currentHitPoints = 100;
        [SerializeField, Tooltip("The number of hit points the shields will regenerate per second.")]
        float regenerationPerSecond = 3;
        
        public float HitPoints
        {
            get { return currentHitPoints; }
            set {
                currentHitPoints = Mathf.Clamp(value, 0, maxHitPoints); 
            }
        }

        /// <summary>
        /// Attempt to regenerate shield amound for the time provided.
        /// </summary>
        /// <param name="time"></param>
        internal void Regenerate(float time)
        {
            HitPoints += regenerationPerSecond * time;
        }
    }
}
