using UnityEngine;
using System.Collections.Generic;
using System;
using DoubTech.OpenPath.Data.Resources;
using DoubTech.OpenPath.Data.Equipment;
using DoubTech.OpenPath.Data.Factions;
using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.Eqipment;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Ship Controller is the Bridge of a ship. It is from here all aspects of a Ship can be
    /// accessed and managed.
    /// </summary>
    public class ShipController : AbstractActionController
    {
        [SerializeField, Tooltip("The faction this ship belongs to. This will be used to decide relations between this ship and other ships and planets.")]
        internal Faction faction;
        [SerializeField, Tooltip("A list of resources this ship is interested in.")]
        internal List<ProductionResource> resources;

        [Header("Captain")]
        [SerializeField, Tooltip("How aggressive is the captain of this ship. This will influence the decision making of the AI ship controllers."), Range(0f, 1f)]
        internal float aggression = 0.5f;

        [SerializeField] public ShipGameEvent onShipInfoChanged;

        private float credits;

        public override float Credits
        {
            get => credits;
            set
            {
                if (value != credits)
                {
                    credits = value;
                    onShipInfoChanged?.Invoke(this);
                }
            }
        }

        public ShipMovementController MovementController { get; internal set; }

        public ShipDamageController DamageController { get; internal set; }

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
        /// <summary>
        /// The primary weapon controller.
        /// </summary>
        public ShipWeaponController WeaponController { get; internal set; }
        /// <summary>
        /// All the weapon controllers on the ship.
        /// </summary>
        public ShipWeaponController[] AllWeaponControllers { get; internal set; }

        internal override void Start()
        {
            base.Start();
            MovementController = GetComponent<ShipMovementController>();
            MiningController = GetComponent<MiningController>();
            TradeController = GetComponent<TradeController>();
            CargoController = GetComponent<CargoController>();
            WeaponController = GetComponent<ShipWeaponController>();
            AllWeaponControllers = GetComponents<ShipWeaponController>();
            DamageController = GetComponent<ShipDamageController>();
        }

        /// <summary>
        /// Add a quantity of credits to the available credits to the ship.
        /// </summary>
        /// <param name="amount"></param>
        public void AddCredits(float amount)
        {
            this.credits += amount;
            onShipInfoChanged?.Invoke(this);
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
            onShipInfoChanged?.Invoke(this);
        }

        /// <summary>
        /// Equip an item in the ship.
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns>True if succesfully equipped otherwise false.</returns>
        internal bool Equip(AbstractShipEquipment equipment)
        {
            bool success = false;
            if (equipment is CargoPod)
            {
                equipment.owner = this;
                success = CargoController.Equip((CargoPod)equipment);
            } else if (equipment is AbstractShipWeapon)
            {
                equipment.owner = this;
                success = WeaponController.Equip((AbstractShipWeapon)equipment);
            } else if (equipment is ShipShieldEquipment)
            {
                equipment.owner = this;
                success = DamageController.Equip((ShipShieldEquipment)equipment);
            }

            if(!success) Debug.LogError($"Ship Controller doesn't know how to equip a {equipment.name}");
            else onShipInfoChanged?.Invoke(this);
            return success;
        }

        private void OnEnable()
        {
            onShipInfoChanged?.Invoke(this);
        }
    }
}
