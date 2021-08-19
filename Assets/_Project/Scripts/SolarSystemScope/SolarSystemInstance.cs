/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Data;
using DoubTech.OpenPath.Data.Config;
using DoubTech.OpenPath.Data.Factions;
using DoubTech.OpenPath.Data.SolarSystemScope;
using DoubTech.OpenPath.Data.UniverseScope;
using DoubTech.OpenPath.Orbits;
using DoubTech.OpenPath.Scenes;
using DoubTech.OpenPath.UniverseScope;
using DoubTech.OpenPath.UniverseScope.Equipment;
using DoubTech.OpenPath.UniverseScope.Resources;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubTech.OpenPath.SolarSystemScope
{
    public class SolarSystemInstance : MonoBehaviour
    {
        [SerializeField] private Vector2 coordinates;
        [SerializeField] private bool runtimeBuild = true;

        private PlanetInstance[] planets;

        public PlanetInstance[] Planets => planets;

        private void Start()
        {
            if (runtimeBuild)
            {
                BuildSolarSystem(SceneConfiguration.currentCoordinates);
            }
        }

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

            var starConfig = GameManager.Instance.galaxyConfig.solarSystemConfig.GetStarConfig(coordinates);
            var star = Instantiate(starConfig.StarPrefab);
            star.StarConfig = starConfig;
            star.transform.parent = transform;

            var planetPositions = GameManager.Instance.galaxyConfig.solarSystemConfig.GetPlanetPositions(coordinates);
            var planets = new PlanetInstance[planetPositions.Length];
            for (int i = 0; i < planetPositions.Length; i++)
            {
                float distance = planetPositions[i];
                PlanetConfig config = GameManager.Instance.galaxyConfig.solarSystemConfig.GetPlanetConfig(coordinates, i, distance);
                planets[i] = CreatePlanetInstance(coordinates, i, config, distance);
                GeneratePlanetGO(star, planets[i].orbit, config);

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    EditorUtility.SetDirty(planets[i]);
                }
#endif
            }

            this.planets = planets;
        }

        internal PlanetInstance CreatePlanetInstance(Vector2 coordinates, int i, PlanetConfig config, float distance)
        {
            Orbit orbit = CreatePlanetOrbit(coordinates, i, distance);
            PlanetInstance planetInstance = orbit.orbitingObjectContainer.gameObject.GetComponent<PlanetInstance>();
            planetInstance.Init(orbit, $"S{coordinates.x}.{this.coordinates.y} P{i}", config);
            planetInstance.planetData.faction = GetOwningFaction(planetInstance.planetData);

            return planetInstance;
        }

        /// <summary>
        /// Decide if the planet is owned by a faction and, if it is, return that faction.
        /// </summary>
        /// <param name="planetInstance">That planer we are testing for</param>
        /// <returns>The owning faction or null if independent.</returns>
        private Faction GetOwningFaction(PlanetData planetData)
        {
            //TODO planets with resource are of interest and might be owned by factions
            if (planetData.population > 0
                || planetData.Habitability > 0.7f)
            {
                if (Random.value < GameManager.Instance.factionConfig.factionDensity)
                {
                    return GameManager.Instance.factionConfig.GetRandomFaction();
                } else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private static void GeneratePlanetGO(StarInstance star, Orbit orbit, PlanetConfig config)
        {
            GameObject planetGO = Instantiate(config.Prefab);

            LightSource lightSource = planetGO.GetComponent<LightSource>();
            if (lightSource) lightSource.Sun = star.gameObject;

            planetGO.transform.parent = orbit.orbitingObjectContainer;
            planetGO.transform.localPosition = Vector3.zero;
            planetGO.transform.localEulerAngles = Vector3.zero;
        }

        private Orbit CreatePlanetOrbit(Vector2 coordinates, int i, float distance)
        {
            Orbit orbit;
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                orbit = Instantiate(GameManager.Instance.galaxyConfig.solarSystemConfig.planetOrbitPrefab);
            }
            else
            {
                orbit = (Orbit)PrefabUtility.InstantiatePrefab(GameManager.Instance.galaxyConfig.solarSystemConfig.planetOrbitPrefab);
            }
#else
                orbit = Instantiate(GameManager.Instance.galaxyConfig.solarSystemConfig.planetOrbitPrefab);
#endif
            orbit.name = $"Planet Orbit for S{coordinates.x}.{this.coordinates.y} P{i}";
            orbit.transform.parent = transform;
            orbit.ellipse.radiusX = distance *
                                    GameManager.Instance.galaxyConfig.solarSystemConfig.distanceScale;
            orbit.ellipse.radiusY = .75f * orbit.ellipse.radiusX;
            orbit.startPosition = Random.Range(0, 1f);
            orbit.orbitingObjectContainer.transform.localPosition = Vector3.zero;
            orbit.RefreshOrbits();
            return orbit;
        }
    }
}
