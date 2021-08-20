/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Data.Resources;
using DoubTech.OpenPath.UniverseScope.Resources;
using UnityEngine;

namespace DoubTech.OpenPath.UniverseScope
{
    public class ResourceSource : MonoBehaviour
    {
        [SerializeField, Tooltip("A resource that can be mined from this planet.")]
        internal ProductionResource resource;
        [SerializeField, Tooltip("The remaining resource quantity that can be mined.")]
        internal float resourceAvailable = 1000000;
        [SerializeField, Tooltip("The quantity of this resource that is mined per second when using standard mining equipment.")]
        internal float quantityPerSecond = 1;

        /// <summary>
        /// Check if there is resource can be mined.
        /// </summary>
        public bool CanBeMined { 
            get { return resourceAvailable > 0; } 
        }

        /// <summary>
        /// The resource that can be mined.
        /// </summary>
        public ProductionResource ResourceType { 
            get { return resource; } 
        }

        /// <summary>
        /// Mine a quantity of this resource using standard mining equipment.
        /// </summary>
        /// <param name="batchDuration">The duration of this extraction batch in seconds.</param>
        /// <returns>The quantity mined.</returns>
        public float Extract(float batchDuration)
        {
            if (!CanBeMined) return 0;

            float quantityMined = quantityPerSecond * batchDuration;
            resourceAvailable -= quantityMined;
            return quantityMined;
        }
    }
}