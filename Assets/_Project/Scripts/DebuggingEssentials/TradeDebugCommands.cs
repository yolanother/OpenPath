using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DebuggingEssentials;
using Sirenix.OdinInspector;
using DoubTech.OpenPath.Eqipment;

namespace DoubTech.OpenPath.Debugging
{
    /// <summary>
    /// Debugging Essentials commands for testing the trade functionality of the game.
    /// </summary>
    [ConsoleAlias("test.trading")]
    public class TradeDebugCommands : AbstractDebugCommands<TradeController>
    {
        public ShipShieldEquipment shields;

        [Button(), HideInEditorMode]
        [ConsoleCommand("trade", "Trade mined resources for maximized profit.")]
        public void SellMostProfitableResource()
        {
            controller.SellLargestRevenueResource();
        }

        [Button(), HideInEditorMode]
        [ConsoleCommand("BuyShields", "Buy shields for the players ship.")]
        public void BuyShields()
        {
            controller.Credits += 10000;
            controller.Buy(shields, 10000);
        }
    }
}
