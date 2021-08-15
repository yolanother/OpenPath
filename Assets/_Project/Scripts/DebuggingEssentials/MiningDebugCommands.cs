using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DoubTech.OpenPath.Debugging
{
    using DebuggingEssentials;
    using DoubTech.OpenPath.Controllers;
    using DoubTech.OpenPath.UniverseScope;
    using Sirenix.OdinInspector;

    /// <summary>
    /// Debugging Essentials commands for testing the mining functionality of the game.
    /// </summary>
    [ConsoleAlias("test.mining")]
    public class MiningDebugCommands : AbstractDebugCommands<MiningController>
    {

        [Button(), HideInEditorMode]
        [ConsoleCommand(description = "Mine the nearest resource that is within range.")]
        public void MineNearestResource()
        {
            Debug.Log("Mining");

            controller.Mine();
        }
    }
}
