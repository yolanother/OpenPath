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
        public Transform player;

        protected T controller;

        private void Awake()
        {
            RuntimeConsole.Register(this);
            controller = player.GetComponent<T>();
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
