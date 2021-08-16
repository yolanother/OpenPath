/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Data;
using DoubTech.OpenPath.Data.Config;
using DoubTech.OpenPath.Data.SolarSystemScope;
using DoubTech.OpenPath.Data.UniverseScope;
using DoubTech.OpenPath.Orbits;
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
            planets = new PlanetInstance[planetPositions.Length];
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
                    orbit = (Orbit)PrefabUtility.InstantiatePrefab(solarSystemConfig.planetOrbitPrefab);
                }
#else
                orbit = Instantiate(solarSystemConfig.planetOrbitPrefab);
#endif

                orbit.name = $"Planet Orbit for S{coordinates.x}.{this.coordinates.y} P{i}";

                orbit.transform.parent = transform;
                var config = solarSystemConfig.GetPlanetConfig(coordinates, i, planetPositions[i]);
                var planet = Instantiate(config.Prefab);
                var lightSource = planet.GetComponent<LightSource>();
                if (lightSource) lightSource.Sun = star.gameObject;
                orbit.ellipse.radiusX = planetPositions[i] *
                                        solarSystemConfig.distanceScale;
                orbit.ellipse.radiusY = .75f * orbit.ellipse.radiusX;

                orbit.startPosition = Random.Range(0, 1f);

                orbit.orbitingObjectContainer.transform.localPosition = Vector3.zero;
                orbit.RefreshOrbits();
                planet.transform.parent = orbit.orbitingObjectContainer;
                planet.transform.localPosition = Vector3.zero;
                planet.transform.localEulerAngles = Vector3.zero;

                var planetInstance = orbit.orbitingObjectContainer.gameObject.GetComponent<PlanetInstance>();
                planetInstance.name = $"S{coordinates.x}.{this.coordinates.y} P{i}";
                if (null == planetInstance.planetData) planetInstance.planetData = new Planet();
                if (Random.value <= config.habitability) planetInstance.planetData.Population = (int)Random.Range(10, 1000000);
                planetInstance.planetData.PlanetId = planetInstance.name;
                planetInstance.planetData.Habitability = config.habitability;
                planetInstance.orbit = orbit;
                planetInstance.planetData.PlanetIndex = i;

                GenerateResourceSupplyAndDemand(config, planetInstance);
                GenerateTrade(config, planetInstance);
                GenerateInvestments(config, planetInstance);

                planets[i] = planetInstance;

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    EditorUtility.SetDirty(planetInstance);
                }
#endif
            }
        }

        private void GenerateTrade(PlanetConfig config, PlanetInstance planetInstance)
        {
            if (planetInstance.planetData.Population <= 0) return;

            float chance = 0;
            for (int i = 0; i < solarSystemConfig.equipment.Length; i++)
            {
                chance = 40 + planetInstance.planetData.Population / 1000;
                if (chance > 0 && Random.value <= chance)
                {
                    EquipmentTrade trade = planetInstance.gameObject.AddComponent<EquipmentTrade>();
                    trade.equipment = solarSystemConfig.equipment[i];
                    trade.quantityAvailable = Random.Range(0, 5);
                    trade.askMultiplier = Random.Range(0.8f, 2f);
                    trade.quantityRequested = Random.Range(0, 5);
                    trade.offerMultiplier = Random.Range(0.2f, 1.1f);
                }
            }
        }

        private void GenerateInvestments(PlanetConfig config, PlanetInstance planetInstance)
        {
            float chance = 0;
            for (int i = 0; i < solarSystemConfig.investments.Length; i++)
            {
                chance = solarSystemConfig.investments[i].Chance(planetInstance.planetData);
                if (chance > 0 && Random.value <= chance)
                {
                    InvestmentOpportunity opportunity = planetInstance.gameObject.AddComponent<InvestmentOpportunity>();
                    opportunity.investment = solarSystemConfig.investments[i];
                }
            }
        }

        private void GenerateResourceSupplyAndDemand(PlanetConfig config, PlanetInstance planetInstance)
        {
            float chance = 0;
            for (int r = 0; r < solarSystemConfig.resources.Length; r++)
            {
                chance = solarSystemConfig.resources[r].generationChance;
                for (int p = 0; p < config.resourceModifiers.Length; p++)
                {
                    if (config.resourceModifiers[p].resource == solarSystemConfig.resources[r])
                    {
                        chance += config.resourceModifiers[p].modificationPercent;
                        break;
                    }
                }

                ResourceSource source = null;
                ResourceDemand demand = null;
                if (chance > 0 && Random.value <= chance)
                {
                    source = planetInstance.gameObject.AddComponent<ResourceSource>();
                    source.resource = solarSystemConfig.resources[r];
                    source.quantityPerSecond = 1; // how easy is it to extract
                    source.resourceAvailable = 50000; // total resource reserves
                }

                if (planetInstance.planetData.Population > 0)
                {
                    demand = planetInstance.gameObject.AddComponent<ResourceDemand>();
                    demand.resource = solarSystemConfig.resources[r];

                    if (source == null)
                    {
                        demand.required = planetInstance.planetData.Population / 1000f;
                    } else
                    {
                        demand.required = planetInstance.planetData.Population / 10000f;
                    }
                }
            }
        }
    }
}
