using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.Resources;

namespace DoubTech.OpenPath.UniverseScope
{
    /// <summary>
    /// A quantity of a resource that has been mined and can be transported, traded and processed.
    /// </summary>
    public struct MinedResource
    {
        public DoubTech.OpenPath.Data.Resources.ProductionResource type;
        public float quantity;

        public MinedResource(ProductionResource resource, float amount)
        {
            this.type = resource;
            this.quantity = amount;
        }

        public override string ToString()
        {
            return string.Format("{0} of {1}", quantity, type.name);
        }
    }
}
