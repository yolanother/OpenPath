using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.Data.Factions;

namespace DoubTech.OpenPath
{
    /// <summary>
    /// The game manager should be present in all scenes and is a central place for data that
    /// is essential to the game at each level of play.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The seed at the root of the generation of the game universe.")] 
        int seed = 0;
        [SerializeField, Tooltip("The Galaxy Config is at the root of the definition of a galaxy and everything within it." +
            "It is essential to generating data within a glaxy.")]
        internal GalaxyConfig galaxyConfig;
        [SerializeField, Tooltip("The Solar System Instance used to generate the solar systems within the game.")]
        internal SolarSystemInstance solarSystemInstance;
        [SerializeField, Tooltip("The factions that exist in this galaxy.")]
        internal FactionConfiguration factionConfig;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public int GetSolarSystemSeed(Vector2 solarSystemCoordinates)
        {
            return (int)(seed +
                            (solarSystemCoordinates.x * galaxyConfig.galaxySize +
                            solarSystemCoordinates.y));
        }
    }
}
