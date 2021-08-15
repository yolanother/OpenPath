using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DoubTech.OpenPath.Debugging
{
    using DebuggingEssentials;
    using DoubTech.OpenPath.Controllers;
    using DoubTech.OpenPath.UniverseScope;

    /// <summary>
    /// Debugging Essentials commands for testing the mining functionality of the game.
    /// </summary>
    [ConsoleAlias("test")]
    public class MiningDebugCommands : MonoBehaviour
    {
        MiningController controller;

        private void Awake()
        {
            RuntimeConsole.Register(this);
            controller = GameObject.FindObjectOfType<MiningController>();
        }

        [ConsoleCommand("mine", "Mine the nearest resource that is within range.")]
        public void Mine()
        {
            Debug.Log("Mining");

            controller.Mine();
        }

        [ConsoleCommand("status", "Return a string describing the current status of the mining controller")]
        public string Status()
        {
            return controller.StatusAsString();
        }
    }
}
