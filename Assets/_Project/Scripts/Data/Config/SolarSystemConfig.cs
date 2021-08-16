/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System.Collections.Generic;
using DoubTech.OpenPath.Data.Config;
using DoubTech.OpenPath.Data.Equipment;
using DoubTech.OpenPath.Data.Resources;
using DoubTech.OpenPath.Data.SolarSystemScope;
using DoubTech.OpenPath.Orbits;
using UnityEngine;

namespace DoubTech.OpenPath.Data
{
    [CreateAssetMenu(fileName = "SolarSystemConfig", menuName = "OpenPath/Config/Solar System Config")]
    public class SolarSystemConfig : ScriptableObject
    {
        [SerializeField] public GalaxyConfig galaxyConfig;
        [SerializeField] public int minPlanets = 5;
        [SerializeField] public int maxPlanets = 20;
        [SerializeField] public int minDistanceBetweenPlanets = 1;
        [SerializeField] public int maxDistanceBetweenPlanets = 5;
        [SerializeField] public int minMoons = 0;
        [SerializeField] public int maxMoons = 3;
        [SerializeField] public int minStarSize = 1;
        [SerializeField] public int maxStarSize = 10;
        [SerializeField] public StarConfig[] starConfigs;
        [SerializeField] public PlanetConfig[] planetConfigs;
        [SerializeField, Tooltip("The production resources that exist in this solar system that can be mined and sold.")]
        internal ProductionResource[] resources;
        [SerializeField, Tooltip("The equipment that exist in this solar system and can be traded.")]
        internal AbstractShipEquipment[] equipment;
        [SerializeField] public float distanceScale = 10;
        [SerializeField] public Orbit planetOrbitPrefab;

        public int GetSeed(Vector2 coordinates)
        {
            return (int) (galaxyConfig.seed +
                                  (coordinates.x * galaxyConfig.galaxySize +
                                   coordinates.y));
        }

        public int GetPlanetCount(Vector2 coordinates)
        {
            var seed = GetSeed(coordinates);
            Random.InitState(seed + 1);
            return Random.Range(minPlanets, maxPlanets);
        }

        public StarConfig GetStarConfig(Vector2 coordinates)
        {
            var seed = GetSeed(coordinates);
            Random.InitState(seed + 2);
            return starConfigs[(int) (Random.value * starConfigs.Length)];
        }

        public int GetStarSize(Vector2 coordinates)
        {
            var seed = GetSeed(coordinates);
            Random.InitState(seed + 3);
            return Random.Range(minStarSize, maxStarSize);
        }

        public float[] GetPlanetPositions(Vector2 coordinates)
        {
            float[] positions = new float[GetPlanetCount(coordinates)];
            Random.InitState(GetSeed(coordinates) + 4);
            float distance = GetStarSize(coordinates);

            for (int i = 0; i < positions.Length; i++)
            {
                distance += Random.Range(minDistanceBetweenPlanets, maxDistanceBetweenPlanets);
                positions[i] = distance;
            }

            return positions;
        }

        public PlanetConfig GetPlanetConfig(Vector2 coordinates, int index, float distance = -1)
        {
            if (distance == -1)
            {
                distance = GetPlanetPositions(coordinates)[index];
            }
            Random.InitState(GetSeed(coordinates) + 5);
            List<PlanetConfig> possiblePlanets = new List<PlanetConfig>();
            // TODO: Index this and improve search
            foreach (var planetConfig in planetConfigs)
            {
                if (planetConfig.maxSpawnDistanceFromSun == 0 &&
                    planetConfig.minSpawnDistanceFromSun == 0)
                {
                    possiblePlanets.Add(planetConfig);
                }
                else if (distance >= planetConfig.minSpawnDistanceFromSun &&
                         distance <= planetConfig.maxSpawnDistanceFromSun)
                {
                    possiblePlanets.Add(planetConfig);
                }
            }

            Random.InitState(GetSeed(coordinates) + 5 + index);
            var planetIndex = Random.Range(0, possiblePlanets.Count - 1);
            return possiblePlanets[planetIndex];
        }
    }
}
