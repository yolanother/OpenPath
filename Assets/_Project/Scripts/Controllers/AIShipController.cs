using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Data.Resources;

namespace DoubTech.OpenPath.Data.UniverseScope
{
    public class AIShipController : ShipController
    {
        internal override void Start()
        {
            base.Start();
            Debug.LogFormat("AI {0} is now active in the solar system.", gameObject.name);
            StartCoroutine(MineTradeLoopCo());
        }

        private IEnumerator MineTradeLoopCo()
        {
            yield return null;

            int resourceIdx = 0;
            while (true)
            {
                ProductionResource resource = resources[resourceIdx];
                MiningController.ConfigureMiningEquipment(resource);

                MiningController.Mine();

                float timeout = Time.realtimeSinceStartup + 20;
                yield return new WaitUntil(() => Time.realtimeSinceStartup > timeout || CargoController.SpaceFor(resource) == 0);

                TradeController.SellLargestRevenueResource();
                timeout = Time.realtimeSinceStartup + 20;
                yield return new WaitUntil(() => Time.realtimeSinceStartup > timeout || CargoController.Quantity(resource) == 0);

                resourceIdx++;
                if (resourceIdx >= resources.Count) resourceIdx = 0;
            }
        }
    }
}
