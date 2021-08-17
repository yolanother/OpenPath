using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Resources;
using System;
using System.Text;
using UnityEngine.Serialization;
using DoubTech.OpenPath.Data.Equipment;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// The Cargo Controller manages all cargo carried within the ship. It is through this controller
    /// that cargo is added and removed from the ship.
    /// </summary>
    public class CargoController : AbstractActionController
    {
        [SerializeField, Tooltip("The Cargo Bays available.")]
        List<CargoPod> cargoPods = new List<CargoPod>();

        MiningController miningController;

        internal override void Start()
        {
            base.Start();
            miningController = GetComponent<MiningController>();
        }

        /// <summary>
        /// Get the total cargo capacity for this ship. This is not available capacity, it includes
        /// capacity already taken.
        /// </summary>
        /// <returns>The total capacity of all storage pods - excluding ancilliary storage such as Mining Equipment</returns>
        public float TotalCapacity
        {
            get
            {
                float total = 0;
                for (int i = 0; i < cargoPods.Count; i++)
                {
                    total += cargoPods[i].capacity;
                }

                return total;
            }
        }

        /// <summary>
        /// Return the amount of available space for a resource. This will include any spare
        /// capacity in a pod configured for this resource and empty pods. It does not include space in mining equipment.
        /// </summary>
        /// <param name="resource">The resource we are interested in</param>
        /// <returns>The capacity available</returns>
        internal float SpaceFor(ProductionResource resource)
        {
            float space = 0;
            for (int i = 0; i < cargoPods.Count; i++)
            {
                if (cargoPods[i].quantity == 0)
                {
                    space += cargoPods[i].capacity;
                } else if (cargoPods[i].resource == resource)
                {
                    space += cargoPods[i].capacity - cargoPods[i].quantity;
                }
            }

            if (miningController != null)
            {
                space += miningController.capacity;
            }

            return space;
        }

        public override string StatusAsString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} cargo pods installed.", cargoPods.Count));
            for (int i = 0; i < cargoPods.Count; i++)
            {
                if (cargoPods[i].resource == null)
                {
                    sb.AppendLine(string.Format("Pod {0} not configured yet.", i));
                }
                else
                {
                    sb.AppendLine(string.Format("Pod {0} configured for {1} and contains {2}/{3}", i, cargoPods[i].resource.name, cargoPods[i].quantity, cargoPods[i].capacity));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Check to see if the ship currently has a store of a specific resource.
        /// </summary>
        /// <param name="resource">The resource we are checking for.</param>
        /// <param name="minQuantity">The minimum quantity required, if not supplied any quantity above 1 will result in a true result.</param>
        /// <returns>True if the resource is present in some quantity</returns>
        public bool Has(ProductionResource resource, float minQuantity = 1)
        {
            if (miningController != null && miningController.resource.type == resource)
            {
                minQuantity -= miningController.resource.quantity;
                if (minQuantity <= 0) return true;
            }

            while (minQuantity > 0)
            {
                for (int i = 0; i < cargoPods.Count; i++)
                {
                    if (cargoPods[i].resource == resource)
                    {
                        minQuantity -= cargoPods[i].quantity;
                        if (minQuantity <= 0) return true;
                    }
                }
                return false;
            }

            return true;
        }

        internal bool Equip(CargoPod pod)
        {
            cargoPods.Add(pod);
            return true;
        }

        internal float Quantity(ProductionResource resource)
        {
            float quantity = 0;
            if (miningController != null && miningController.resource.type == resource)
            {
                quantity += miningController.resource.quantity;
            }

            for (int i = 0; i < cargoPods.Count; i++)
            {
                if (cargoPods[i].resource == resource) quantity += cargoPods[i].quantity;
            }

            return quantity;
        }

        /// <summary>
        /// Stow an amount of a resource in a cargo pod.
        /// </summary>
        /// <param name="resource">The resource to be stowed.</param>
        /// <param name="quantity">The quantity of the resource to be stowed. If there is insufficient cargo space to stow this amount af the resource as much as possible will be stowed and the remainder will be returned.</param>
        /// <returns>The amount of the resource that remains unstowed. That is if there is insufficient cargo space to stow the full quantitye the return value will be the unstowed amount, otherwsie it will be zero.</returns>
        internal float Stow(ProductionResource resource, float quantity)
        {
            for (int i = 0; i < cargoPods.Count; i++)
            {
                quantity = cargoPods[i].Stow(resource, quantity);
                if (quantity == 0)
                {
                    return 0;
                }
            }

            return quantity;
        }

        internal void Remove(ProductionResource resource, float quantity)
        {
            while (quantity > 0)
            {
                if (miningController != null && miningController.resource.type == resource)
                {
                    if (miningController.resource.quantity > quantity)
                    {
                        miningController.resource.quantity = miningController.resource.quantity - quantity;
                        quantity = 0;
                        break;
                    } else
                    {
                        quantity -= miningController.resource.quantity;
                        miningController.resource.quantity = 0;
                    }
                }

                for (int i = 0; i < cargoPods.Count; i++)
                {
                    if (cargoPods[i].resource == resource)
                    {
                        if (cargoPods[i].quantity > quantity)
                        {
                            cargoPods[i].Remove(quantity);
                            quantity = 0;
                            break;
                        } else
                        {
                            quantity -= cargoPods[i].quantity;
                            cargoPods[i].Empty();
                        }
                    }
                }
            }
        }
    }
}
