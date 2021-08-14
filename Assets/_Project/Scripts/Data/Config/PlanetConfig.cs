/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using UnityEngine;

namespace DoubTech.OpenPath.Data.Config
{
    [CreateAssetMenu(fileName = "PlanetConfig", menuName = "OpenPath/Config/Planet Config")]
    public class PlanetConfig : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] public int minSpawnDistanceFromSun;
        [SerializeField] public int maxSpawnDistanceFromSun;

        public GameObject Prefab => prefab;
    }
}
