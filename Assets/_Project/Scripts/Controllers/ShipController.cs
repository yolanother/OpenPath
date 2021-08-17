using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DoubTech.OpenPath.Data.Resources;
using DoubTech.OpenPath.Data.Equipment;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Ship Controller is the Bridge of a ship. It is from here all aspects of a Ship can be
    /// accessed and managed.
    /// </summary>
    public class ShipController : AbstractActionController
    {
        [SerializeField, Tooltip("A list of resources this ship is interested in.")]
        internal List<ProductionResource> resources;

        private float credits;

        public override float Credits { 
            get => credits; 
            set => credits = value; }

        public ShipMovementController MovementController { get; internal set; }

        /// <summary>
        /// Search through the resources this ship cares about and, if found, 
        /// return a resource to represent it.
        /// </summary>
        /// <param name="name">The name of the resource we are looking for.</param>
        /// <returns>The resource object matching the name or null if no match found.</returns>
        internal ProductionResource GetResource(string name)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].name == name)
                {
                    return resources[i];
                }
            }

            return null;
        }

        public MiningController MiningController { get; internal set; }

        public override string StatusAsString()
        {
            return string.Format("{0} Credits", credits);
        }

        public TradeController TradeController { get; internal set; }
        public CargoController CargoController { get; internal set; }

        internal override void Start()
        {
            base.Start();
            MovementController = GetComponent<ShipMovementController>();
            MiningController = GetComponent<MiningController>();
            TradeController = GetComponent<TradeController>();
            CargoController = GetComponent<CargoController>();
        }

        /// <summary>
        /// Add a quantity of credits to the available credits to the ship.
        /// </summary>
        /// <param name="amount"></param>
        public void AddCredits(float amount)
        {
            this.credits += amount;
        }

        /// <summary>
        /// Remove a quantity of credirs from the ships account.
        /// </summary>
        /// <param name="amount"></param>
        internal void RemoveCredits(float amount)
        {
            if (credits < amount)
            {
                throw new ArgumentException("Attempted to remove more credits from ship account than exist in the account.");
            }
            this.credits -= amount;
        }

        /// <summary>
        /// Equip an item in the ship.
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns>True if succesfully equipped otherwise false.</returns>
        internal bool Equip(AbstractShipEquipment equipment)
        {
            if (equipment is CargoPod)
            {
                return CargoController.Equip((CargoPod)equipment);
            }

            return false;
        }
    }
}
