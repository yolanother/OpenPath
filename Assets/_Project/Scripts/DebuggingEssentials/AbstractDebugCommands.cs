using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DebuggingEssentials;
using Sirenix.OdinInspector;
using DoubTech.OpenPath.Controllers;

namespace DoubTech.OpenPath.Debugging
{
    public abstract class AbstractDebugCommands<T> : MonoBehaviour where T : AbstractActionController
    {
        [SerializeField, Tooltip("How frequently, in seconds, should this debugger update its status? " +
            "When updating status the debugger will query the game status. For some debuggers this is " +
            "a costly operation and so you should slow down the frequency of updates.")]
        internal float updateFrequency;

        internal Transform player;

        protected T controller;

        private void Awake()
        {
            RuntimeConsole.Register(this);
        }

        private void Start()
        {
            StartCoroutine(RefreshShips());
        }

        private IEnumerator RefreshShips()
        {
            yield return null;
            while (true)
            {
                ShipController[] ships = GameManager.FindObjectsOfType<ShipController>();
                foreach (ShipController ship in ships)
                {
                    if (ship != null && ship.CompareTag("Player")) // need to check for nulla as may have been destroyed since above FindObjectsOfType
                    {
                        player = ship.transform;
                        controller = player.GetComponent<T>();
                    }
                    yield return true;
                }
                yield return new WaitForSeconds(updateFrequency);
            }
        }

        [InfoBox("$Status")]
        public string description;

        /// <summary>
        /// Return a string representing the current status of this debugger. The results of this will be displayed as an infobox.
        /// </summary>
        public string Status
        {
            get { return StatusAsString(); }
        }

        [ConsoleCommand("status", "Return a string describing the current status of the controller being tested.")]
        public string StatusAsString()
        {
            if (controller != null)
            {
                return controller.StatusAsString();
            } else
            {
                return "Enter play mode to enable debug commands.";
            }
        }
    }
}
