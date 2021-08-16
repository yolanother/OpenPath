using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DebuggingEssentials;
using Sirenix.OdinInspector;

namespace DoubTech.OpenPath.Debugging
{
    /// <summary>
    /// Debugging Essentials commands for testing the trade functionality of the game.
    /// </summary>
    [ConsoleAlias("test.trading")]
    public class TradeDebugCommands : AbstractDebugCommands<TradeController>
    {
        [Button(), HideInEditorMode]
        [ConsoleCommand("trade", "Trade mined resources for maximized profit.")]
        public void SellMostProfitableResource()
        {
            controller.SellLargestRevenueResource();
        }
    }
}
