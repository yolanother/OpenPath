using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Investments;
using DoubTech.OpenPath.SolarSystemScope;

namespace DoubTech.OpenPath.Data.UniverseScope
{
    /// <summary>
    /// Attach InvestmentOpportunity components to planets to indicate to characters that there are investment opportunities on that planet.
    /// </summary>
    public class InvestmentOpportunity : MonoBehaviour
    {
        [SerializeField, Tooltip("The kind of investment opportunity available.")]
        internal PlanetInvestment investment;
        [SerializeField, Tooltip("The number of opportunties available.")]
        internal int numberOfOpportunities = 1;

        /// <summary>
        /// Invest the required amount of money into this scheme.
        /// The number of opportunities avilable will be decreased
        /// and any effects will be applied.
        /// </summary>
        public void Invest()
        {
            GetComponent<PlanetInstance>().planetData.HealthcareQuality += investment.healthcareIncrease;

            numberOfOpportunities--;
            if (numberOfOpportunities <= 0)
            {
                Destroy(this);
            }

            Debug.LogFormat("Invested {0} in {1} at {3}.", investment.requiredInvestmentAmount, investment.name, gameObject.name);
        }
    }
}
