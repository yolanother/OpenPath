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
    public class MiningController : MonoBehaviour
    {
        [SerializeField, Tooltip("The range a player needs to be from a source in order to be able to mine it.")]
        float maximumRange = 10;
        [SerializeField, Tooltip("The maximum capacity of this mining controller. Once the mined resource hits" +
            "this capacity mining will stop and cannot be restarted until the mining equipment has been emptied.")]
        float capacity = 1000;

        ShipController shipController;
        private MinedResource resource;

        private void Start()
        {
            shipController = GetComponentInParent<ShipController>();
        }

        internal string StatusAsString()
        {
            return string.Format("Currently have {0} of {1} mined. Capacity is {2}.", resource.quantity, resource.type.name, capacity);
        }

        /// <summary>
        /// Find the nearest resource source that has the same type of resource currently in the mining equipment (if any) and attempt to mine it.
        /// If no resource type already present then find the nearest resource of any type and mine it.
        /// </summary>
        /// <returns>A MinedResource object representing the mined resource if succesful. Null if mining is unsuccessful for any reason.</returns>
        public void Mine()
        {
            if (resource.type != null && resource.quantity >= capacity) return;

            float minDistance = float.MaxValue;
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, maximumRange, hitColliders);
            float distance;
            ResourceSource source = null;
            ResourceSource candidate;
            for (int i = 0; i < numColliders; i++)
            {
                candidate = hitColliders[i].GetComponent<ResourceSource>();
                if (candidate != null && (resource.type == null || candidate.Type == resource.type))
                {
                    if (candidate != null)
                    {
                        distance = Vector3.Distance(transform.position, candidate.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            source = candidate;
                        }
                    }
                }
            }

            if (source != null)
            {
                StartCoroutine(MineResourceSourceCo(source));
            }
        }

        IEnumerator MineResourceSourceCo(ResourceSource source)
        {
            resource = new MinedResource(source.Type, 0);

            shipController.MoveToOrbit(source, 1.5f);
            while (!shipController.InPosition)
            {
                yield return new WaitForEndOfFrame();
            }

            while (source.ResourceAvailable && capacity - resource.quantity > 0)
            {
                if (source.ResourceAvailable)
                {
                    resource.quantity += source.Mine();
                }
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
