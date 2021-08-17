using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Factions;
using DoubTech.OpenPath.Data.UniverseScope;
using DoubTech.OpenPath.Events;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

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
