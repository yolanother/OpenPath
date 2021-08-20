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
using DoubTech.OpenPath.Data.Config;
using DoubTech.OpenPath.Data.Equipment;
using DoubTech.OpenPath.Data.SolarSystemScope;
using DoubTech.OpenPath.Data.UniverseScope;
using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.Orbits;
using DoubTech.OpenPath.UniverseScope;
using DoubTech.OpenPath.UniverseScope.Equipment;
using DoubTech.OpenPath.UniverseScope.Resources;
using UnityEngine;

namespace DoubTech.OpenPath.SolarSystemScope
{
    public class PlanetInstance : MonoBehaviour, IDamageController
    {
        public Orbit orbit;
        public PlanetData planetData = new PlanetData();
        private HashSet<ShipController> orbitingShips = new HashSet<ShipController>();

        public ShipController[] OrbitingShips => orbitingShips.ToArray();
        public bool HasShipInOrbit => orbitingShips.Count > 0;
        public bool IsPlayerOrbiting => orbitingShips.Contains(PlayerShip.Instance.shipController);

        public void IsOrbiting(ShipController ship) => orbitingShips.Contains(ship);

        float timeOfNextTick;
        PlanetConfig config;

        internal void Init(Orbit orbit, string name, PlanetConfig config)
        {
            this.orbit = orbit;
            this.name = name;
            this.config = config;
            if (null == planetData)
            {
                planetData = new PlanetData();
            }

            planetData.PlanetId = name;

            planetData.Habitability = config.habitability;
            if (Random.value <= config.habitability)
            {
                planetData.Population = (int)Random.Range(10, 1000000);
            }

            GenerateResourceSupplyAndDemand();
            GenerateTrade();
            GenerateInvestments();
            GenerateFactionAffinities();
        }

        //TODO right now factions are given an entirely random affinity. We should make it relative to the distance to a home planet.
        private void GenerateFactionAffinities()
        {
            float highestAffinity = 0;

            //TODO planets with resource are of interest and might be owned by factions
            if (planetData.Population > 0
                || (planetData.Habitability > 0.7f && Random.value > 0.6f))
            {
                Data.Factions.FactionConfiguration factionsConfig = GameManager.Instance.factionConfig;
                for (int i = 0; i < factionsConfig.aiFactions.Length; i++)
                {
                    float affinity = Random.Range(-1f, 1f);
                    planetData.factionAffinity.Add(factionsConfig.aiFactions[i], affinity);
                    if (affinity >= highestAffinity)
                    {
                        planetData.owningFaction = factionsConfig.aiFactions[i];
                    }
                }
            } else
            {
                planetData.owningFaction = null;
            }
        }

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

        private void GenerateResourceSupplyAndDemand()
        {
            float chance = 0;
            for (int r = 0; r < GameManager.Instance.galaxyConfig.solarSystemConfig.resources.Length; r++)
            {
                chance = GameManager.Instance.galaxyConfig.solarSystemConfig.resources[r].generationChance;
                for (int p = 0; p < config.resourceModifiers.Length; p++)
                {
                    if (config.resourceModifiers[p].resource == GameManager.Instance.galaxyConfig.solarSystemConfig.resources[r])
                    {
                        chance += config.resourceModifiers[p].modificationPercent;
                        break;
                    }
                }

                ResourceSource source = null;
                ResourceDemand demand = null;
                if (chance > 0 && Random.value <= chance)
                {
                    source = gameObject.AddComponent<ResourceSource>();
                    source.resource = GameManager.Instance.galaxyConfig.solarSystemConfig.resources[r];
                    source.quantityPerSecond = 1; // how easy is it to extract
                    source.resourceAvailable = Random.Range(10000, 50000); // total resource reserves
                }

                if (planetData.Population > 0)
                {
                    demand = gameObject.AddComponent<ResourceDemand>();
                    demand.resource = GameManager.Instance.galaxyConfig.solarSystemConfig.resources[r];

                    if (source == null)
                    {
                        demand.requiredQuantity = planetData.Population / 1000f;
                    }
                    else
                    {
                        demand.requiredQuantity = planetData.Population / 10000f;
                    }
                }
            }
        }
        private void GenerateTrade()
        {
            if (planetData.Population <= 0) return;

            float chance = 0;
            for (int i = 0; i < GameManager.Instance.galaxyConfig.solarSystemConfig.equipment.Length; i++)
            {
                chance = 40 + planetData.Population / 1000;
                if (chance > 0 && Random.value <= chance)
                {
                    EquipmentTrade trade = gameObject.AddComponent<EquipmentTrade>();
                    trade.equipment = GameManager.Instance.galaxyConfig.solarSystemConfig.equipment[i];
                    trade.quantityAvailable = Random.Range(0, 5);
                    trade.askMultiplier = Random.Range(0.8f, 2f);
                    trade.quantityRequested = Random.Range(0, 5);
                    trade.offerMultiplier = Random.Range(0.2f, 1.1f);
                }
            }
        }

        private void GenerateInvestments()
        {
            float chance = 0;
            for (int i = 0; i < GameManager.Instance.galaxyConfig.solarSystemConfig.investments.Length; i++)
            {
                chance = GameManager.Instance.galaxyConfig.solarSystemConfig.investments[i].Chance(planetData);
                if (chance > 0 && Random.value <= chance)
                {
                    InvestmentOpportunity opportunity = gameObject.AddComponent<InvestmentOpportunity>();
                    opportunity.investment = GameManager.Instance.galaxyConfig.solarSystemConfig.investments[i];
                }
            }
        }

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
