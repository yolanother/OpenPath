using DoubTech.OpenPath.Data.Resources;
using UnityEngine;

namespace DoubTech.OpenPath.Data.Equipment
{
    [CreateAssetMenu(fileName = "Cargo Pod", menuName = "OpenPath/Config/Equipment")]
    public class CargoPod : ScriptableObject
    {
        public ProductionResource resource;
        public float quantity;
        public float capacity;

        public CargoPod(ProductionResource resource, float capacity)
        {
            this.resource = resource;
            this.capacity = capacity;
            this.quantity = 0;
        }

        internal void Empty()
        {
            this.quantity = 0;
        }

        /// <summary>
        /// The amount of space currently available in this cargo pod.
        /// </summary>
        internal float Space
        {
            get { return capacity - quantity; }
        }

        /// <summary>
        /// Stow an amount of the required resource in this cargo pod,  converting the pod to the
        /// right resource pod if necessary.
        /// </summary>
        /// 
        /// <param name="quantity"></param>
        /// <returns></returns>
        internal float Stow(ProductionResource resource, float quantity)
        {
            if (this.resource != resource)
            {
                if (this.quantity == 0)
                {
                    this.resource = resource;
                }
                else
                {
                    return quantity;
                }
            }

            if (Space > quantity)
            {
                this.quantity += quantity;
                return 0;
            }
            else
            {
                float excess = quantity - Space;
                this.quantity = capacity;
                return excess;
            }
        }

        internal void Remove(float quantity)
        {
            this.quantity -= quantity;
        }
    }
}
