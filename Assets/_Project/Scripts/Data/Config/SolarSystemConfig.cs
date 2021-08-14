/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Data.Config;
using DoubTech.OpenPath.Data.SolarSystemScope;
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

        public int GetSeed(Vector2 coordinates)
        {
            return (int) (galaxyConfig.seed +
                                  (coordinates.x * galaxyConfig.galaxySize +
                                   coordinates.y));
        }

        public int PlanetCount(Vector2 coordinates)
        {
            var seed = GetSeed(coordinates);
            Random.InitState(seed + 1);
            return Random.Range(minPlanets, maxPlanets);
        }

        public StarConfig GetStarConfig(Vector2 coordinates)
        {
            var seed = GetSeed(coordinates);
            Random.InitState(seed + 2);
            return starConfigs[Random.Range(0, starConfigs.Length - 1)];
        }

        public int GetStarSize(Vector2 coordinates)
        {
            var seed = GetSeed(coordinates);
            Random.InitState(seed + 3);
            return Random.Range(minStarSize, maxStarSize);
        }

        public float GetPlanetPosition(Vector2 coordinates, int index)
        {
            Random.InitState(GetSeed(coordinates) + 4);
            float distance = GetStarSize(coordinates);
            for (int i = 0; i < index; i++)
            {
                distance += Random.Range(minDistanceBetweenPlanets, maxDistanceBetweenPlanets);
            }

            return distance;
        }
    }
}
