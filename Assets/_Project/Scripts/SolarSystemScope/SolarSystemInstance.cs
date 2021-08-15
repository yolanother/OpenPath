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
using DoubTech.OpenPath.Orbits;
using Sirenix.OdinInspector;
using UnityEditor;
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
            var star = Instantiate(starConfig.StarPrefab);
            star.StarConfig = starConfig;
            star.transform.parent = transform;

            var planetPositions = solarSystemConfig.GetPlanetPositions(coordinates);
            for (int i = 0; i < planetPositions.Length; i++)
            {
                Orbit orbit;
                #if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    orbit = Instantiate(solarSystemConfig.planetOrbitPrefab);
                }
                else
                {
                    orbit = (Orbit) PrefabUtility.InstantiatePrefab(solarSystemConfig.planetOrbitPrefab);
                }
                #else
                orbit = Instantiate(solarSystemConfig.planetOrbitPrefab);
                #endif

                orbit.transform.parent = transform;
                var config = solarSystemConfig.GetPlanetConfig(coordinates, i, planetPositions[i]);
                var planet = Instantiate(config.Prefab);
                var lightSource = planet.GetComponent<LightSource>();
                if(lightSource) lightSource.Sun = star.gameObject;
                orbit.ellipse.radiusX = planetPositions[i] *
                                        solarSystemConfig.distanceScale;
                orbit.ellipse.radiusY = .75f * orbit.ellipse.radiusX;

                orbit.startPosition = Random.Range(0, 1f);

                orbit.orbitingObjectContainer.transform.localPosition = Vector3.zero;
                orbit.RefreshOrbits();
                planet.transform.parent = orbit.orbitingObjectContainer;
                planet.transform.localPosition = Vector3.zero;
                planet.transform.localEulerAngles = Vector3.zero;
            }
        }
    }
}
