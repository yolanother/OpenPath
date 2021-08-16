using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using System;
using DoubTech.OpenPath.Data.Investments;
using DoubTech.OpenPath.Data.UniverseScope;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// Planet Investmen Controller allows planets to be improved through investment. Improved planets
    /// can yield more resources, equipment and population.
    /// </summary>
    public class PlanetInvestmentController : AbstractController
    {
        private Coroutine investmentCo;
        private ShipMovementController shipMovementController;
        private float investmentDuration = 2f;

        internal override void Start()
        {
            base.Start();
            shipMovementController = shipController.MovementController;
        }

        public override string StatusAsString()
        {
            return "Ready to invest";
        }

        internal void InvestInHealthcare(int credits)
        {
            if (this.Credits < credits) return;

            List<InvestmentOpportunity> candidates = ScanForPlanetsOfType<InvestmentOpportunity>();

            InvestmentOpportunity investment = null;
            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidates[i].investment.healthcareIncrease > 0 && candidates[i].investment.requiredInvestmentAmount <= credits)
                {
                    investment = candidates[i];
                }
            }

            if (investment != null)
            {
                investmentCo = StartCoroutine(InvestCo(investment));
            }
            else
            {
                Debug.LogErrorFormat("Unable to find a planet requiring healthcare investment of under {1} within scan range of {0}", transform.position, credits);
            }
        }

        IEnumerator InvestCo(InvestmentOpportunity opportunity)
        {
            shipMovementController.MoveToOrbit(opportunity);
            while (!InPosition(opportunity.transform.position))
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(investmentDuration);

            Credits -= opportunity.investment.requiredInvestmentAmount;
            opportunity.Invest();
        }
    }
}
