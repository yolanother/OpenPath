using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Factions;
using DoubTech.OpenPath.Data.UniverseScope;
using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.Scenes;
using DoubTech.OpenPath.SolarSystemScope;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;
using DoubTech.OpenPath.Data;
using DoubTech.OpenPath.Data.Config;
using DoubTech.OpenPath.Data.SolarSystemScope;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Faction Controller is responsible for managing faction activities within the solarsystem.
    /// </summary>
    public class FactionController : MonoBehaviour
    {
        [SerializeField, Tooltip("The frequency at which the FactionController should consider spawning new enemy ships. This will be randomize +/-25% to remove predictability.")]
        float shipSpawnFrequency = 1;
        [SerializeField, Tooltip("Event to fire when an enemy ship is spawned.")]
        ShipGameEvent OnSpawnShipGameEvent;

        [ShowInInspector, ReadOnly]
        internal List<AIShipController> activeShips = new List<AIShipController>();

        private List<Faction> factionsPresent = new List<Faction>();



        private void Start()
        {
            SolarSystemInstance solarSystemInstance = GetComponent<SolarSystemInstance>();
            Random.InitState(GameManager.Instance.GetSolarSystemSeed(SceneConfiguration.currentCoordinates));

            SolarSystemConfig systemConfig = GameManager.Instance.galaxyConfig.solarSystemConfig;
            float[] planetDistances = systemConfig.GetPlanetPositions(SceneConfiguration.currentCoordinates);
            for (int i = 0; i < planetDistances.Length; i++)
            {
                PlanetConfig config = systemConfig.GetPlanetConfig(SceneConfiguration.currentCoordinates, i, planetDistances[i]);
                //FIXME this will result in multiple planet instances being created, we need a Get method to replace the create this will create if it doesn't already exist
                PlanetInstance planetInstance = GameManager.Instance.SolarSystemInstance.CreatePlanetInstance(SceneConfiguration.currentCoordinates, i, config, planetDistances[i]);
                PlanetData planetData = planetInstance.planetData;

                if (planetData.owningFaction != null)
                {
                    if (planetData.owningFaction != null)
                    {
                        factionsPresent.Add(planetData.owningFaction);
                    }
                }
            }

            StartCoroutine(SpawnShips());
        }

        private IEnumerator SpawnShips()
        {
            yield return null;

            while (true)
            {
                if (activeShips.Count < 5)
                {
                    AIShipController ship = factionsPresent[Random.Range(0, factionsPresent.Count)].SpawnShip();
                    activeShips.Add(ship);
                    OnSpawnShipGameEvent?.Invoke(ship);
                }

                yield return new WaitForSeconds(Random.Range(shipSpawnFrequency * 0.75f, shipSpawnFrequency * 1.25f));
            }
        }
    }
}
