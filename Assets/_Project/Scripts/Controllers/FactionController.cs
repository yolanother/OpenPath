using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Factions;
using DoubTech.OpenPath.Data.UniverseScope;
using Sirenix.OdinInspector;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Faction Controller is responsible for managing faction activities within the solarsystem.
    /// </summary>
    public class FactionController : MonoBehaviour
    {
        [SerializeField, Tooltip("The faction this controller is responsible for.")]
        Faction faction;

        [ShowInInspector, ReadOnly]
        internal List<AIShipController> activeShips = new List<AIShipController>();

        private void Update()
        {
            if (activeShips.Count < 3)
            {
                AIShipController ship = faction.SpawnShip();
                activeShips.Add(ship);
            }
        }
    }
}
