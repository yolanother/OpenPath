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
        public Planet planetData = new Planet();

        float timeOfNextTick;

        private void Update()
        {
            if (Time.timeSinceLevelLoad >= timeOfNextTick) {
                planetData.Tick();
                timeOfNextTick = Time.timeSinceLevelLoad + planetData.tickFrequency;
            }
        }
    }
}
