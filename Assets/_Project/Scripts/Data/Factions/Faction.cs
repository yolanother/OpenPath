/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Data.UniverseScope;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubTech.OpenPath.Data.Factions
{
    [CreateAssetMenu(fileName = "Faction", menuName = "OpenPath/Factions/Faction")]
    public class Faction : ScriptableObject
    {
        [SerializeField] public Color factionColor;
        [PreviewField(200, ObjectFieldAlignment.Right)]
        [SerializeField] public Sprite factionEmblem;
        [SerializeField, Tooltip("A normalized level of aggression. This controls the likelyhood of AIs in this faction taking agressive action, such as attacking other ships and planets.")]
        float aggression = 0.3f;
        [SerializeField, Tooltip("The ship prefabs to use for this faction.")]
        AIShipController[] shipPrefabs;

        /// <summary>
        /// Spawn a faction ship at random.
        /// </summary>
        /// <returns>The spawned ship</returns>
        internal AIShipController SpawnShip()
        {
            AIShipController ship = Instantiate(shipPrefabs[Random.Range(0, shipPrefabs.Length)]);
            ship.transform.position = Random.insideUnitSphere * 500;
            ship.faction = this;
            ship.name = $"Captain { NameList.Random} of { name }";

            return ship;
        }
    }
}
