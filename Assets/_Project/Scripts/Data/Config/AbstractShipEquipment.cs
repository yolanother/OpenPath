using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;

namespace DoubTech.OpenPath.Data.Equipment
{
    public abstract class AbstractShipEquipment : ScriptableObject
    {
        [SerializeField, Tooltip("The base value of this resource in the Universe. This will be modified by local factors such as scarcity.")]
        internal float baseValue = 100;
        [SerializeField, Tooltip("The current owner of this equipment.")]
        internal ShipController owner;
    }
}