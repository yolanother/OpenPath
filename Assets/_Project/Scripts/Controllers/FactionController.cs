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

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Faction Controller is responsible for managing faction activities within the solarsystem.
    /// </summary>
    public class FactionController : MonoBehaviour
    {
        [SerializeField, Tooltip("The faction this controller is responsible for.")]
        private FactionConfiguration factionConfiguration;

        [ShowInInspector, ReadOnly]
        internal List<AIShipController> activeShips = new List<AIShipController>();

        [SerializeField] private ShipGameEvent onShipChanged;
        private Faction faction;

        private void Start()
        {
            SolarSystemInstance solarSystemInstance = GetComponent<SolarSystemInstance>();
            Random.InitState(GameManager.Instance.GetSolarSystemSeed(SceneConfiguration.currentCoordinates));
            faction = factionConfiguration.aiFactions[
                Random.Range(0, factionConfiguration.aiFactions.Length - 1)];
        }

        private void Update()
        {
            if (activeShips.Count < 3)
            {
                AIShipController ship = faction.SpawnShip();
                activeShips.Add(ship);
                onShipChanged?.Invoke(ship);
            }
        }
    }
}
