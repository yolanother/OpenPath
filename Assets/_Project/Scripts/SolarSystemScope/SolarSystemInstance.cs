/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections.Generic;
using DoubTech.OpenPath.Data;
using DoubTech.OpenPath.Data.Config;
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

            #if UNITY_EDITOR
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            #endif

            var starConfig = solarSystemConfig.GetStarConfig(coordinates);
            var star = Instantiate(starConfig.StarPrefab).transform;
            star.parent = transform;

            var planetPositions = solarSystemConfig.GetPlanetPositions(coordinates);
            for (int i = 0; i < planetPositions.Length; i++)
            {
                var config = solarSystemConfig.GetPlanetConfig(coordinates, i, planetPositions[i]);
                var planet = Instantiate(config.Prefab);
                planet.transform.parent = transform;
                planet.transform.position = planetPositions[i] * Vector3.forward * solarSystemConfig.distanceScale;
                var lightSource = planet.GetComponent<LightSource>();
                if(lightSource) lightSource.Sun = star.gameObject;
            }
        }
    }
}
