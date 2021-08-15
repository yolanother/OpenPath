/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.UniverseScope;
using UnityEngine;

namespace DoubTech.OpenPath.Data.SolarSystemScope
{
    [CreateAssetMenu(fileName = "StarConfig", menuName = "OpenPath/Config/Star Config")]
    public class StarConfig : ScriptableObject
    {
        [SerializeField] private StarAppearenceManager starPrefab;
        [SerializeField] public Color starColor;
        [SerializeField] public float hdrMultiplier = 10;

        public StarAppearenceManager StarPrefab => starPrefab;
    }
}
