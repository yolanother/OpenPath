using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Equipment;
using System;

namespace DoubTech.OpenPath.UniverseScope.Equipment
{
    /// <summary>
    /// Equipment Offer indicates that a particular piece of equipment is available for sale or purchase at a location.
    /// Players and NPCs will be able to buy and sell equipment here if they can come to agreement
    /// on price.
    /// </summary>
    public class EquipmentOffer : MonoBehaviour
    {
        [SerializeField, Tooltip("Equipment that is offered at this location.")]
        internal AbstractShipEquipment equipment;
        [SerializeField, Tooltip("The remaining quantity available.")]
        internal float quantityAvailable = 2;
        [SerializeField, Tooltip("The multiplier to apply to the base price when calculating the asking price.")]
        float askMultiplier = 1f;
        [SerializeField, Tooltip("The quantity this location is willing to purchase.")]
        internal float quantityRequested = 2;
        [SerializeField, Tooltip("The multiplier to apply to the base price when calculating the offered price.")]
        float offerMultiplier = 0.4f;

        internal float AskingPrice
        {
            get {
                Debug.LogWarning("TODO: Calculate price to ask for equipment based on local ecopnomic factors.");
                return equipment.baseValue * askMultiplier; 
            }
        }

        internal float OfferPrice
        {
            get
            {
                Debug.LogWarning("TODO: Calculate price to offer for equipment based on local ecopnomic factors.");
                return equipment.baseValue * offerMultiplier;
            }
        }

        /// <summary>
        /// Complete an exchange of credits for equipment.
        /// </summary>
        internal AbstractShipEquipment Buy()
        {
            quantityAvailable--;
            Debug.LogWarning("TODO: when completing an equipment offer we should add credits to the offerer.");
            return ScriptableObject.Instantiate(equipment);
        }
    }
}
