using UnityEngine;
using DoubTech.OpenPath.Data;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.Data.Factions;
using DoubTech.OpenPath.Controllers;

namespace DoubTech.OpenPath
{
    /// <summary>
    /// The game manager should be present in all scenes and is a central place for data that
    /// is essential to the game at each level of play.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Is this GameManager persistent across scene loads?")]
        public bool isPersistant = true;

        [Header("Game Configuration")]
        [SerializeField, Tooltip("The prefab used to create the player if they do not already exist in the scene.")]
        ShipController playerPrefab;

        [Header("Universe Generation")]
        [SerializeField, Tooltip("The seed at the root of the generation of the game universe.")]
        int seed = 0;
        [SerializeField, Tooltip("The Galaxy Config is at the root of the definition of a galaxy and everything within it." +
            "It is essential to generating data within a glaxy.")]
        internal GalaxyConfig galaxyConfig;
        [SerializeField, Tooltip("The Solar System Instance used to generate the solar systems within the game.")]
        SolarSystemInstance solarSystemInstance;
        [SerializeField, Tooltip("The factions that exist in this galaxy.")]
        internal FactionConfiguration factionConfig;

        public static GameManager Instance { get; private set; }

        public ShipController player { get; private set; }

        public SolarSystemInstance SolarSystemInstance {
            get {
                if (!solarSystemInstance)
                {
                    //OPTIMIZATION this will be called once per scene after scene load. We should move this into a OnSceneLoad or similar place
                    solarSystemInstance = GameObject.FindObjectOfType<SolarSystemInstance>();
                }
                return solarSystemInstance;
            }
        }

        public virtual void Awake()
        {
            if (isPersistant)
            {
                if (!Instance)
                {
                    Instance = this;
                }
                else
                {
                    Destroy(gameObject);
                }
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Instance = this;
            }

            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO)
            {
                player = playerGO.GetComponent<ShipController>();
            } else {
                player = Instantiate(playerPrefab);
            }
            DontDestroyOnLoad(player.gameObject);
        }

        public int GetSolarSystemSeed(Vector2 solarSystemCoordinates)
        {
            return (int)(seed +
                            (solarSystemCoordinates.x * galaxyConfig.galaxySize +
                            solarSystemCoordinates.y));
        }
    }
}
