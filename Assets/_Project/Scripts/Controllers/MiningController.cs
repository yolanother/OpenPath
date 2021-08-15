using DoubTech.OpenPath.Data.Resources;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UniverseScope;
using System;
using System.Collections;
using System.Text;
using UnityEngine;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// Controls the ability to mine resource materials from a resource source.
    /// </summary>
    public class MiningController : AbstractController
    {
        [SerializeField, Tooltip("The range a player needs to be from a source in order to be able to use sensors to detect available resources.")]
        float maximumSensorRange = 10;
        [SerializeField, Tooltip("The range a player must be from a source of resources to be able to mine it.")]
        float maximumMiningRange = 2;
        [SerializeField, Tooltip("The maximum capacity of this mining controller. Once the mined resource hits" +
            "this capacity mining will stop and cannot be restarted until the mining equipment has been emptied.")]
        float capacity = 1000;
        [SerializeField, Tooltip("The time between mining extractions in seconds. The longer this is the longer between the addition of " +
            "resources to the stored quantity.")]
        float batchDuration = 0.25f;

        ShipController shipController;
        ShipMovementController shipMovementController;
        internal MinedResource resource;
        private Coroutine miningCo;

        private void Start()
        {
            shipController = GetComponentInParent<ShipController>();
            shipMovementController = shipController.MovementController;
        }

        public override string StatusAsString()
        {
            if (resource.type == null)
            {
                return "No resource type assigned to the mining controller yet.";
            }
            else
            {
                return string.Format("Currently have {0} of {1} mined. Capacity is {2}.", resource.quantity, resource.type.name, capacity);
            }
        }

        /// <summary>
        /// Find the nearest resource source that has the same type of resource currently in the mining equipment (if any) and attempt to mine it.
        /// If no resource type already present then find the nearest resource of any type and mine it.
        /// Mined resources will be stored within the mining controller. Once full mining will stop.
        /// </summary>
        public void Mine()
        {
            StopMining();

            if (resource.type != null && resource.quantity >= capacity) return;

            float minDistance = float.MaxValue;
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, maximumSensorRange, hitColliders);
            float distance;
            ResourceSource source = null;
            ResourceSource candidate;
            for (int i = 0; i < numColliders; i++)
            {
                candidate = hitColliders[i].GetComponent<ResourceSource>();
                if (candidate != null && (resource.type == null || (candidate.ResourceType == resource.type && resource.quantity < capacity)))
                {
                    distance = Vector3.Distance(transform.position, candidate.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        source = candidate;
                    }
                }
            }

            if (source != null)
            {
                miningCo = StartCoroutine(MineResourceSourceCo(source));
            }
        }

        IEnumerator MineResourceSourceCo(ResourceSource source)
        {
            if (resource.type == null)
            {
                resource = new MinedResource(source.ResourceType, 0);
            } else
            {
                Debug.Log("Converting Mining Equipment to mine " + source.ResourceType + " any exiting resources in the equipment will be jetisoned.");
                resource = new MinedResource(source.ResourceType, 0);
            }
            shipMovementController.MoveToOrbit(source, 1.5f);
            while (!shipMovementController.InPosition)
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(batchDuration);

            while (source.ResourceAvailable && capacity - resource.quantity > 0)
            {
                if (source.ResourceAvailable)
                {
                    resource.quantity += source.Extract(batchDuration);
                }
                yield return new WaitForSeconds(batchDuration);

                if (Vector3.SqrMagnitude(transform.position - source.transform.position) > maximumMiningRange * maximumMiningRange)
                {
                    StopMining();
                }
            }
        }

        /// <summary>
        /// Stop the current mining process if there is one and move any resource in the quipment into cargo pods if possible.
        /// </summary>
        private void StopMining()
        {
            if (miningCo != null)
            {
                StopCoroutine(miningCo);
            }
            resource.quantity = shipController.CargoController.Stow(resource.type, resource.quantity);
        }
    }
}
