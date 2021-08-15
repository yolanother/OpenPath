using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Ship Controller is the Bridge of a ship. It is from here all aspects of a Ship can be
    /// accessed and managed.
    /// </summary>
    public class ShipController : AbstractController
    {
        private float credits;

        public ShipMovementController MovementController { get; internal set; }
        public MiningController MiningController { get; internal set; }

        public override string StatusAsString()
        {
            return string.Format("{0} Credits", credits);
        }

        public TradeController TradeController { get; internal set; }
        public CargoController CargoController { get; internal set; }

        private void Awake()
        {
            MovementController = GetComponent<ShipMovementController>();
            MiningController = GetComponent<MiningController>();
            TradeController = GetComponent<TradeController>();
            CargoController = GetComponent<CargoController>();
        }

        /// <summary>
        /// Add a quantity of credits to the available credits to the ship.
        /// </summary>
        /// <param name="credits"></param>
        public void AddCredits(float credits)
        {
            this.credits += credits;
        }
    }
}
