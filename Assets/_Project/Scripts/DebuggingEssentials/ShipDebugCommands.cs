using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using Sirenix.OdinInspector;
using DebuggingEssentials;
using DoubTech.OpenPath.Data.Resources;

namespace DoubTech.OpenPath.Debugging
{
    [ConsoleAlias("test.ship")]
    public class ShipDebugCommands : AbstractDebugCommands<ShipController>
    {
        [HideInEditorMode, Button()]
        [ConsoleCommand("Mine Iron, Sell Iron, Buy Cargo Pod, Mine Gold, Sell Gold")]
        public void MineTradeEquipLoop()
        {
            string firstResourceName = "Iron";
            string secondResourceName = "Gold";
            StartCoroutine(MineTradeEquip(firstResourceName, secondResourceName));
        }

        private IEnumerator MineTradeEquip(string firstResourceName, string secondResourceName)
        {
            // Mine Iron
            ProductionResource resource = controller.GetResource(firstResourceName);
            if (resource != null)
            {
                controller.MiningController.ConfigureMiningEquipment(resource);
            }
            else
            {
                Debug.LogErrorFormat("Ship is not aware of resource named {0}", firstResourceName);
            }

            controller.MiningController.Mine();

            float timeout = Time.realtimeSinceStartup + 10;
            yield return new WaitUntil(() => Time.realtimeSinceStartup > timeout || controller.CargoController.Quantity(resource) > 3);

            // Sell Iron
            controller.TradeController.SellLargestRevenueResource();
            timeout = Time.realtimeSinceStartup + 10;
            yield return new WaitUntil(() => Time.realtimeSinceStartup > timeout || controller.CargoController.Quantity(resource) == 0);

            // Buy Cargo Pod
            float originalCapacity = controller.CargoController.TotalCapacity;
            string cargoPod = "Cargo Pod";
            if (controller.TradeController.Buy(cargoPod))
            {
                timeout = Time.realtimeSinceStartup + 10;
                yield return new WaitUntil(() => Time.realtimeSinceStartup > timeout || controller.CargoController.TotalCapacity > originalCapacity);
            } else
            {
                Debug.LogError("Didn't manage to buy a Cargo Pod");
            }

            if (controller.CargoController.TotalCapacity <= originalCapacity)
            {
                Debug.LogError("Total cargo capacity did not increase when attempting to buy a Cargo Pod.");
            }

            // Mine Gold
            resource = controller.GetResource(secondResourceName);
            if (resource != null)
            {
                controller.MiningController.ConfigureMiningEquipment(resource);
            }
            else
            {
                Debug.LogErrorFormat("Ship is not aware of resource named {0}", secondResourceName);
            }
            controller.MiningController.Mine();

            timeout = Time.realtimeSinceStartup + 10;
            yield return new WaitUntil(() => Time.realtimeSinceStartup > timeout || controller.CargoController.Quantity(resource) > 0.5);

            // Sell Gold
            controller.TradeController.SellLargestRevenueResource();
            timeout = Time.realtimeSinceStartup + 10;
            yield return new WaitUntil(() => Time.realtimeSinceStartup > timeout || controller.CargoController.Quantity(resource) == 0);
        }
    }
}
