/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System.Collections.Generic;
using System.Linq;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Data.SolarSystemScope;
using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.Orbits;
using UnityEngine;

namespace DoubTech.OpenPath.SolarSystemScope
{
    public class PlanetInstance : MonoBehaviour
    {
        public Orbit orbit;
        public Planet planetData = new Planet();
        private HashSet<ShipController> orbitingShips = new HashSet<ShipController>();

        public ShipController[] OrbitingShips => orbitingShips.ToArray();
        public bool HasShipInOrbit => orbitingShips.Count > 0;
        public bool IsPlayerOrbiting => orbitingShips.Contains(PlayerShip.Instance.shipController);

        public void IsOrbiting(ShipController ship) => orbitingShips.Contains(ship);

        float timeOfNextTick;

        private void Update()
        {
            if (Time.timeSinceLevelLoad >= timeOfNextTick) {
                planetData.Tick();
                timeOfNextTick = Time.timeSinceLevelLoad + planetData.tickFrequency;
            }
        }

        public void AddOrbitingShip(ShipController shipController)
        {
            orbitingShips.Add(shipController);
        }

        public void RemoveOrbitingObject(ShipController shipController)
        {
            orbitingShips.Remove(shipController);
        }
    }
}
