using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using System;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// Planet Investmen Controller allows planets to be improved through investment. Improveed planets
    /// can yield more resources, equipment and population.
    /// </summary>
    public class PlanetInvestmentController : AbstractController
    {


        public override string StatusAsString()
        {
            return "Ready to invest";
        }

        internal void InvestInHealthcare(int credits)
        {
            if (this.credits < credits) return;

            // Find the planet to invest in
            
        }
    }
}
