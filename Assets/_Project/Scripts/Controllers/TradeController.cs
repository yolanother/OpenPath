using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.UniverseScope.Resources;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Trade Controller manages trade between entities within the game.
    /// </summary>
    public class TradeController : AbstractController
    {
        [SerializeField, Tooltip("The range a player needs to be from a point of demand in order to be able to initiate a trade.")]
        float maximumRange = 20;
        
        ShipController shipController;
        ShipMovementController shipMovementController;
        CargoController cargoController;
float tradeDuration = 2f;

        private void Start()
        {
            shipController = GetComponent<ShipController>();
            shipMovementController = shipController.MovementController;
            cargoController = shipController.CargoController;
        }

        /// <summary>
        /// A short range scan is made to find locations that have demand for resources available to this trade controller.
        /// If a trade opportunity is found then the ship will move to that location and seek to perform a trade.
        /// </summary>
        public void Trade()
        {
            float maxEstimatedProfit = float.MinValue;
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, maximumRange, hitColliders);
            float distance;
            ResourceDemand demand = null;
            ResourceDemand candidate;
            for (int i = 0; i < numColliders; i++)
            {
                candidate = hitColliders[i].GetComponent<ResourceDemand>();
                if (candidate != null && cargoController.Has(candidate.resource))
                {
                    float available = cargoController.Quantity(candidate.resource);
                    float estimatedProfit;
                    if (available >= candidate.required)
                    {
                        estimatedProfit = candidate.required * candidate.resource.baseValue;
                    }
                    else
                    {
                        estimatedProfit = available * candidate.resource.baseValue;
                    }

                    if (estimatedProfit > maxEstimatedProfit)
                    {
                        maxEstimatedProfit = estimatedProfit;
                        demand = candidate;
                    }
                }
            }

            if (demand != null)
            {
                StartCoroutine(TradeResourceCo(demand));
            }
        }

        IEnumerator TradeResourceCo(ResourceDemand demand)
        {
            shipMovementController.MoveToOrbit(demand, 1.5f);
            while (!shipMovementController.InPosition)
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(tradeDuration);

            float quantity = demand.required > cargoController.Quantity(demand.resource) ? cargoController.Quantity(demand.resource) : demand.required;
            cargoController.Remove(demand.resource, quantity);
            demand.required -= quantity;
            shipController.AddCredits(demand.Price * quantity);
        }

        public override string StatusAsString()
        {
            return "N/A";
        }
    }
}
