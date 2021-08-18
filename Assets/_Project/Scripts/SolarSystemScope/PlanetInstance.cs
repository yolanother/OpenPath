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
using DoubTech.OpenPath.Data.Equipment;
using DoubTech.OpenPath.Data.SolarSystemScope;
using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.Orbits;
using DoubTech.OpenPath.UniverseScope.Equipment;
using UnityEngine;

namespace DoubTech.OpenPath.SolarSystemScope
{
    public class PlanetInstance : MonoBehaviour, IDamageController
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

        #region Orbit Controller

        public void AddOrbitingShip(ShipController shipController)
        {
            orbitingShips.Add(shipController);
        }

        public void RemoveOrbitingObject(ShipController shipController)
        {
            orbitingShips.Remove(shipController);
        }

        #endregion


        #region Damage Controller
        public void AddDamage(AbstractShipWeapon weapon, float damageAmount)
        {
            if (planetData.Population > 0)
            {
                float percentKills = (damageAmount / 10000) * Random.Range(0.5f, 1.5f); ;
                int kills = (int)(percentKills * planetData.Population);
                planetData.Population -= kills;

                if (planetData.Population > 0)
                {
                    Debug.Log($"{weapon.owner} killed {kills} inhabitants of {planetData.DisplayName}, there are {planetData.Population} inhabitants remaining.");
                } else
                {
                    Debug.Log($"{weapon.owner} killed the last inhabitants of {planetData.DisplayName}, it is now avialble for resource stripping or colonization.");
                    planetData.Population = 0;
                    Die();
                }
            }
        }

        public void Die()
        {

        }

        #endregion

        #region Trade Controller

        internal List<EquipmentTrade> GetForSale<T>() where T : AbstractShipEquipment
        {
            List<EquipmentTrade> results = new List<EquipmentTrade>();
            EquipmentTrade[] offers = GetComponents<EquipmentTrade>();
            for (int i = 1; i < offers.Length; i++)
            {
                if (offers[i].equipment is T && offers[i].quantityAvailable > 0)
                {
                    results.Add(offers[i]);
                }
            }

            return results;
        }

        internal List<EquipmentTrade> GetSellable<T>(T equipment = null) where T : AbstractShipEquipment
        {
            List<EquipmentTrade> results = new List<EquipmentTrade>();
            EquipmentTrade[] offers = GetComponents<EquipmentTrade>();
            for (int i = 1; i < offers.Length; i++)
            {
                if (offers[i].equipment is T && offers[i].quantityRequested > 0)
                {
                    if (!equipment || equipment == offers[i].equipment)
                    {
                        results.Add(offers[i]);
                    }
                }
            }

            return results;
        }

        #endregion
    }
}
