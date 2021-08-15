using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Resources;

namespace DoubTech.OpenPath.UniverseScope.Resources
{
    /// <summary>
    /// Resource Demand indicates that a particular resource is in demand at a location.
    /// Players and NPCs will be able to sell resources here if they can come to agreement
    /// on price.
    /// </summary>
    public class ResourceDemand : MonoBehaviour
    {
        [SerializeField, Tooltip("A resource that is demanded at this location.")]
        internal ProductionResource resource;
        [SerializeField, Tooltip("The remaining resource quantity that is demanded.")]
        internal float required = 5000;
        
        internal float Price
        {
            get {
                Debug.LogWarning("TODO: Calculate price to pay for a resource based on local demand and resource scarcity.");
                return resource.baseValue; 
            }
        }
    }
}
