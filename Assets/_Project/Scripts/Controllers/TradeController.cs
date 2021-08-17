using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.UniverseScope.Resources;
using System;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UniverseScope.Equipment;
using DoubTech.ScriptableEvents.BuiltinTypes;
using DoubTech.OpenPath.Data.Equipment;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Trade Controller manages trade between entities within the game.
    /// </summary>
    public class TradeController : AbstractActionController
    {
        [SerializeField] private FloatGameEvent onTradedResources;

        float tradeDuration = 2f;

        internal override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// A short range scan is made to find locations that have demand for resources available to this trade controller.
        /// If a trade opportunity is found then the ship will move to that location and seek to perform a trade.
        /// </summary>
        public void SellLargestRevenueResource()
        {
            Status = State.Preparing;

            List<ResourceDemand> candidates = ScanForObjectsOfType<ResourceDemand>();

            float maxEstimatedRevenue = float.MinValue;
            ResourceDemand demand = null;
            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidates[i] != null && shipController.CargoController.Has(candidates[i].resource))
                {
                    float available = shipController.CargoController.Quantity(candidates[i].resource);
                    float estimatedrevenue;
                    if (available >= candidates[i].required)
                    {
                        estimatedrevenue = candidates[i].required * candidates[i].resource.baseValue;
                    }
                    else
                    {
                        estimatedrevenue = available * candidates[i].resource.baseValue;
                    }

                    if (estimatedrevenue > maxEstimatedRevenue)
                    {
                        maxEstimatedRevenue = estimatedrevenue;
                        demand = candidates[i];
                    }
                }
            }

            if (demand != null)
            {
                Debug.LogFormat("Decided to trade {0} with {1} at an estimated income of {2}", demand.resource.name, demand.name, maxEstimatedRevenue);
                StartCoroutine(TradeResourceCo(demand));
            } else
            {
                Status = State.Inactive;
            }
        }

        public void SellAll(PlanetInstance planetInstance)
        {
            ResourceDemand[] demands = planetInstance.GetComponents<ResourceDemand>();

            foreach (var demand in demands)
            {
                StartCoroutine(TradeResourceCo(demand));
            }
        }

        /// <summary>
        /// Attempt to purchase a named item. If the item is available and is deemed an acceptable price it
        /// will be purchased and credits will be deducted.
        /// </summary>
        /// <param name="name">The item we want to purchase</param>
        /// <returns>True if a purchase offer is available and the ship has begun the transaction, otherwise false.</returns>
        internal bool Buy(AbstractShipEquipment requiredEquipment, float maxUnitPrice, int quantity = 1)
        {
            Status = State.Preparing;

            // Find the most suitable seller
            float minEstimatedCost = float.MaxValue;
            List<EquipmentTrade> candidates = ScanForObjectsOfType<EquipmentTrade>();

            EquipmentTrade offer = null;
            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidates[i] != null 
                    && requiredEquipment == candidates[i].equipment
                    && candidates[i].quantityAvailable > quantity
                    && candidates[i].AskingPrice <= maxUnitPrice)
                {
                    float estimatedCost = candidates[i].AskingPrice;

                    if (estimatedCost < minEstimatedCost)
                    {
                        minEstimatedCost = estimatedCost;
                        offer = candidates[i];
                    }
                }
            }

            if (offer != null)
            {
                Debug.Log($"{gameObject.name} decided to purchase {offer.equipment.name} from {offer.name} at an estimated cost of {minEstimatedCost}");
                StartCoroutine(BuyEquipmentCo(offer));
                return true;
            }
            else
            {
                Debug.Log($"{gameObject.name} unable to find a seller of {requiredEquipment.name} at a max unit price of {maxUnitPrice}.");
                Status = State.Inactive;
                return false;
            }
        }

        IEnumerator BuyEquipmentCo(EquipmentTrade offer)
        {
            Status = State.InProgress;

            shipController.MovementController.MoveToOrbit(offer);
            while (!InPosition(offer.transform.position))
            {
                yield return new WaitForEndOfFrame();
            }

            float cost = offer.AskingPrice;
            shipController.RemoveCredits(cost);
            AbstractShipEquipment equipment = offer.Buy();

            shipController.Equip(equipment);

            Debug.Log($"{gameObject.name} purchased and equipped {offer.equipment.name} from {offer.name} for a cost of {cost}.");

            Status = State.Inactive;
        }

        IEnumerator TradeResourceCo(ResourceDemand demand)
        {

            Status = State.InProgress;

            shipController.MovementController.MoveToOrbit(demand);
            while (!InPosition(demand.transform.position))
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(tradeDuration);

            float quantity = demand.required > shipController.CargoController.Quantity(demand.resource) ? shipController.CargoController.Quantity(demand.resource) : demand.required;
            shipController.CargoController.Remove(demand.resource, quantity);
            demand.required -= quantity;
            float price = demand.Price * quantity;
            shipController.AddCredits(price);
            Debug.LogWarning("TODO: when completing resource sale we should deduct credits to the offerer.");

            Debug.Log($"{gameObject.name} traded {quantity} of {demand.resource.name} for a price of {price}.");

            Status = State.Inactive;
            onTradedResources?.Invoke(quantity);
        }

        public override string StatusAsString()
        {
            return "N/A";
        }

        Color sensorCoverageGizmoColor = new Color(0, 255, 0, 128);
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = sensorCoverageGizmoColor;
            Gizmos.DrawWireSphere(transform.position, maxSensorRange);
        }
    }
}
