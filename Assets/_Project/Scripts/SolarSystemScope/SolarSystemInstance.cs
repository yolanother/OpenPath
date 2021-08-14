/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubTech.OpenPath.SolarSystemScope
{
    public class SolarSystemInstance : MonoBehaviour
    {
        [SerializeField] public SolarSystemConfig solarSystemConfig;
        [SerializeField] private Vector2 coordinates;

        private PlanetInstance[] planets;

        [Button]
        public void BuildSolarSystem(Vector2 coordinates)
        {
            this.coordinates = coordinates;

            var starConfig = solarSystemConfig.GetStarConfig(coordinates);
            Instantiate(starConfig.StarPrefab);

            var planetCount = solarSystemConfig.PlanetCount(coordinates);
            for (int i = 0; i < planetCount; i++)
            {

            }
        }
    }
}
