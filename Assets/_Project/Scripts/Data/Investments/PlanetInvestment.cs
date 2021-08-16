using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.SolarSystemScope;
using System;

namespace DoubTech.OpenPath.Data.Investments
{
    /// <summary>
    /// A Planet Investment is an opportunity to invest in the development of a planet. Investing in planets impacts in a (usually) positive way.
    /// </summary>
    [CreateAssetMenu(fileName = "Planetary Investment", menuName = "OpenPath/Config/Investment")]
    public class PlanetInvestment : ScriptableObject
    {
        [SerializeField, Tooltip("The total amount that must be invested in this scheme.")]
        internal float requiredInvestmentAmount = 10000;
        [SerializeField, Range(0.01f, 1f), Tooltip("Impact on healthcare in terms of the number of normalized points to increase the healthcare quality.")]
        internal float healthcareIncrease = 0;

        /// <summary>
        /// Calculate a chance of this investment being available on a given planet.
        /// </summary>
        /// <param name="planetData">The data object describing the planet.</param>
        /// <returns>A value between 0 (no chance) and 1 (</returns>
        internal float Chance(Planet planetData)
        {
            if (planetData.Population <= 0)
            {
                return 0;
            }

            float chance = 0;
            if (planetData.HealthcareQuality < 1)
            {
                chance +=  0.4f + planetData.Population / 10000;
            }

            return Mathf.Clamp01(chance);

        }
    }
}
