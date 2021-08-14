/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using UnityEngine;

namespace DoubTech.OpenPath.Data.SolarSystemScope
{
    [CreateAssetMenu(fileName = "StarConfig", menuName = "OpenPath/Config/Star Config")]
    public class StarConfig : ScriptableObject
    {
        [SerializeField] private GameObject starPrefab;

        public GameObject StarPrefab => starPrefab;
    }
}
