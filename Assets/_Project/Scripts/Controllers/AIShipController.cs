using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Data.Resources;
using DoubTech.OpenPath.Data.Equipment;

namespace DoubTech.OpenPath.Data.UniverseScope
{
    public class AIShipController : ShipController
    {
        [Header("Captain")]
        [SerializeField, Tooltip("How aggressive is the captain of this ship."), Range(0f, 1f)]
        internal float aggression = 0.5f;

        [Header("Mission")]
        [SerializeField, Tooltip("An ordered list of items that this AI will attempt to buy for their ship. " +
            "The AI will mine and trade until it can afford the first item in the list. It will then purchase " +
            "that item, remove it from the list and repeat.")]
        List<AbstractShipEquipment> desiredEquipment;

        internal override void Start()
        {
            base.Start();
            Debug.LogFormat("AI {0} is now active in the solar system.", gameObject.name);
            StartCoroutine(MainControlerLoopCo());
        }

        private IEnumerator MainControlerLoopCo()
        {
            float timeout;
            WeaponController.OnAlert = true;
            yield return null;

            int resourceIdx = 0;
            while (true)
            {
                // Buy Desired Equpment if possible
                if (desiredEquipment.Count > 0 && shipController.TradeController.Buy(desiredEquipment[0], Credits))
                {
                    timeout = Time.realtimeSinceStartup + 20;
                    yield return new WaitUntil(() => Time.realtimeSinceStartup > timeout || shipController.TradeController.Status == TradeController.State.Inactive);

                    desiredEquipment.RemoveAt(0);
                }

                // Mine resources
                ProductionResource resource = resources[resourceIdx];
                MiningController.ConfigureMiningEquipment(resource);

                WeaponController.OnAlert = false;
                MiningController.Mine();

                timeout = Time.realtimeSinceStartup + 20;
                yield return new WaitUntil(() => Time.realtimeSinceStartup > timeout || CargoController.SpaceFor(resource) == 0);

                // Sell Resources
                TradeController.SellLargestRevenueResource();
                timeout = Time.realtimeSinceStartup + 20;
                yield return new WaitUntil(() => Time.realtimeSinceStartup > timeout || CargoController.Quantity(resource) == 0);

                // Engage enemies
                WeaponController.OnAlert = true;
                yield return new WaitForSeconds(2);

                resourceIdx++;
                if (resourceIdx >= resources.Count) resourceIdx = 0;
            }
        }
    }
}
