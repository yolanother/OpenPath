/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Data.Resources;
using UnityEngine;

namespace DoubTech.OpenPath.UniverseScope
{
    public class ResourceSource : MonoBehaviour
    {
        [SerializeField, Tooltip("A resource that can be mined from this planet.")]
        ProductionResource resource;
        [SerializeField, Tooltip("The remaining resource quantity that can be mined.")]
        float resourceAvailable = 1000000;
        [SerializeField, Tooltip("The quantity of this resource that is mined per second when using standard mining equipment.")]
        float quantityPerSecond = 1;

        /// <summary>
        /// Check if there is resource available to be mined.
        /// </summary>
        public bool ResourceAvailable { 
            get { return resourceAvailable > 0; } 
        }

        /// <summary>
        /// The resource that can be mined.
        /// </summary>
        public ProductionResource Type { 
            get { return resource; } 
        }

        /// <summary>
        /// Mine a quantity of this resource using standard mining equipment.
        /// </summary>
        /// <returns>The quantity mined.</returns>
        public float Mine()
        {
            if (!ResourceAvailable) return 0;

            float quantityMined = quantityPerSecond * Time.deltaTime;
            resourceAvailable -= quantityMined;
            return quantityMined;
        }
    }
}