using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.UniverseScope.Resources;
using System;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UniverseScope.Equipment;
using DoubTech.ScriptableEvents.BuiltinTypes;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Trade Controller manages trade between entities within the game.
    /// </summary>
    public class TradeController : AbstractController
    {
        [SerializeField] private FloatGameEvent onTradedResources;

        ShipMovementController shipMovementController;
        CargoController cargoController;
        float tradeDuration = 2f;

        internal override void Start()
        {
            base.Start();
            shipMovementController = shipController.MovementController;
            cargoController = shipController.CargoController;
        }

        /// <summary>
        /// A short range scan is made to find locations that have demand for resources available to this trade controller.
        /// If a trade opportunity is found then the ship will move to that location and seek to perform a trade.
        /// </summary>
        public void SellLargestRevenueResource()
        {
            List<ResourceDemand> candidates = ScanForPlanetsOfType<ResourceDemand>();

            float maxEstimatedRevenue = float.MinValue;
            ResourceDemand demand = null;
            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidates[i] != null && cargoController.Has(candidates[i].resource))
                {
                    float available = cargoController.Quantity(candidates[i].resource);
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
            }
        }

        public void SellAll(PlanetInstance planetInstance)
        {
            var demands = planetInstance.GetComponents<ResourceDemand>();

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
        internal bool Buy(string name)
        {
            // Find the most suitable seller
            float minEstimatedCost = float.MaxValue;
            int maxColliders = 100;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, maxSensorRange, hitColliders);
            EquipmentTrade offer = null;
            EquipmentTrade candidate;
            for (int i = 0; i < numColliders; i++)
            {
                candidate = hitColliders[i].GetComponentInParent<EquipmentTrade>();
                if (candidate != null)
                {
                    float estimatedCost = candidate.AskingPrice;

                    if (estimatedCost < minEstimatedCost)
                    {
                        minEstimatedCost = estimatedCost;
                        offer = candidate;
                    }
                }
            }

            if (offer != null)
            {
                Debug.LogFormat("Decided to purchase {0} from {1} at an estimated cost of {2}", offer.equipment.name, offer.name, minEstimatedCost);
                StartCoroutine(BuyEquipmentCo(offer));
                return true;
            } else
            {
                Debug.LogFormat("Unable to find a seller of {0}.", name);
                return false;
            }
        }

        IEnumerator BuyEquipmentCo(EquipmentTrade offer)
        {
            shipMovementController.MoveToOrbit(offer);
            while (!InPosition(offer.transform.position))
            {
                yield return new WaitForEndOfFrame();
            }

            float cost = offer.AskingPrice;
            shipController.RemoveCredits(cost);
            offer.Buy();

            shipController.Equip(offer.equipment);

            Debug.LogFormat("Purchased and equipped {0} from {1} for a cost of {2}.", offer.equipment.name, offer.name, cost);
        }

        IEnumerator TradeResourceCo(ResourceDemand demand)
        {
            shipMovementController.MoveToOrbit(demand);
            while (!InPosition(demand.transform.position))
            {
                yield return new WaitForEndOfFrame();
            }

            Debug.Log("Initiating trade");

            yield return new WaitForSeconds(tradeDuration);

            float quantity = demand.required > cargoController.Quantity(demand.resource) ? cargoController.Quantity(demand.resource) : demand.required;
            cargoController.Remove(demand.resource, quantity);
            demand.required -= quantity;
            float price = demand.Price * quantity;
            shipController.AddCredits(price);
            Debug.LogWarning("TODO: when completing resource sale we should deduct credits to the offerer.");

            Debug.LogFormat("Traded {0} of {1} for a price of {2}.", quantity, demand.resource.name, price);
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
