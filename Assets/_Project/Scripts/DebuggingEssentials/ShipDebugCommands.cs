using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using Sirenix.OdinInspector;
using DebuggingEssentials;
using DoubTech.OpenPath.Data.Resources;
using DoubTech.OpenPath.UniverseScope.Equipment;
using DoubTech.OpenPath.Data.Equipment;

namespace DoubTech.OpenPath.Debugging
{
    [ConsoleAlias("test.ship")]
    public class ShipDebugCommands : AbstractDebugCommands<ShipController>
    {
        public AbstractShipEquipment requiredEquipment;

        [HideInEditorMode, Button()]
        [ConsoleCommand("Perform a complete loop of: Mine Iron, Sell Iron, Buy Cargo Pod, Mine Gold, Sell Gold")]
        public void MineTradeEquipLoop()
        {
            string firstResourceName = "Iron";
            string secondResourceName = "Gold";
            StartCoroutine(MineTradeEquip(firstResourceName, secondResourceName, requiredEquipment));
        }

        private IEnumerator MineTradeEquip(string firstResourceName, string secondResourceName, AbstractShipEquipment requiredEquipment)
        {
            // Mine first resource
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

            if (controller.CargoController.Quantity(resource) < 3)
            {
                Debug.LogError("Was unable to mine sufficient iron in the time allowed");
            }

            // Sell resource
            controller.TradeController.SellLargestRevenueResource();
            timeout = Time.realtimeSinceStartup + 10;
            yield return new WaitUntil(() => Time.realtimeSinceStartup > timeout || controller.CargoController.Quantity(resource) == 0);

            // Buy Cargo Pod
            float originalCapacity = controller.CargoController.TotalCapacity;
            if (controller.TradeController.Buy(requiredEquipment, 1000))
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
