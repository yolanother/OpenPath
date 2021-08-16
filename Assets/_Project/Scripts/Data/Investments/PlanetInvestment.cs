using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    }
}
