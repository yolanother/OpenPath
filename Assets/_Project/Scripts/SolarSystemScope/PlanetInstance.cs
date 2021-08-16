/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Data.SolarSystemScope;
using DoubTech.OpenPath.Orbits;
using UnityEngine;

namespace DoubTech.OpenPath.SolarSystemScope
{
    public class PlanetInstance : MonoBehaviour
    {
        public Orbit orbit;
        [SerializeField, Tooltip("The current population of intelligent lifeforms in 10,000's.")]
        internal int population = 0;
        public Planet planetData = new Planet();
    }
}
