using DoubTech.OpenPath.Data.Resources;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UniverseScope;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DoubTech.ScriptableEvents.BuiltinTypes;
using UnityEngine;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// Controls the ability to mine resource materials from a resource source.
    /// </summary>
    public class MiningController : AbstractActionController
    {
        [SerializeField, Tooltip("The maximum capacity of this mining controller. Once the mined resource hits" +
            "this capacity mining will stop and cannot be restarted until the mining equipment has been emptied.")]
        internal float capacity = 5;
        [SerializeField, Tooltip("The time between mining extractions in seconds. The longer this is the longer between the addition of " +
            "resources to the stored quantity.")]
        float batchDuration = 0.25f;

        [SerializeField] private Transform miningBeam;
        [SerializeField] private GameObject miningBeamVisualization;
        [SerializeField] private FloatGameEvent onMinedResources;

        internal MinedResource resource;
        private Coroutine miningCo;

        internal override void Start()
        {
            base.Start();
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
        /// Configure the mining equipment to mine a specific resource type.
        /// Any resource currently in the mining equipment storage will be stowed
        /// if possible, any excess will be dumped.
        /// </summary>
        /// <param name="desiredResource">The resource we want to mine.</param>
        public void ConfigureMiningEquipment(ProductionResource desiredResource)
        {
            if (resource.quantity > 0)
            {
                shipController.CargoController.Stow(resource.type, resource.quantity);
            }

            resource.type = desiredResource;
            resource.quantity = 0;

            Debug.Log("Mining equipment is configured for " + resource.type.name);
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

            List<ResourceSource> candidates = ScanForPlanetsOfType<ResourceSource>();

            float minDistance = float.MaxValue;
            float distance;
            ResourceSource source = null;
            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidates[i] != null && (resource.type == null || (candidates[i].ResourceType == resource.type && resource.quantity < capacity)))
                {
                    if (candidates[i].GetComponent<PlanetInstance>().planetData.Population > 0) continue;

                    distance = Vector3.Distance(transform.position, candidates[i].transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        source = candidates[i];
                    }
                }
            }

            Mine(source);
        }

        public void Mine(ResourceSource source)
        {
            if (source != null)
            {
                miningCo = StartCoroutine(MineResourceSourceCo(source));
            }
            else
            {
                Debug.LogErrorFormat(
                    "Unable to find a mining source for {0} within scanning distance of {1}.",
                    resource.type.name, transform.position);
            }
        }

        IEnumerator MineResourceSourceCo(ResourceSource source)
        {
            yield return null;

            if (resource.type == null)
            {
                resource = new MinedResource(source.ResourceType, 0);
            } else
            {
                resource = new MinedResource(source.ResourceType, 0);
                Debug.Log("Converting Mining Equipment to mine " + source.ResourceType.name + " any exiting resources in the equipment will be jetisoned.");
            }
            shipController.MovementController.MoveToOrbit(source);
            while (!InPosition(source.transform.position))
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(batchDuration);

            currentMinedSource = source;
            isMining = true;
            while (source.ResourceAvailable
                && capacity - resource.quantity > 0
                && InPosition(source.transform.position))
            {
                if (source.ResourceAvailable)
                {
                    float amount = source.Extract(batchDuration);
                    resource.quantity += amount;
                    //Debug.LogFormat("Mined {0} of {1} from {2}.\n\nTotal {1} available is now {3}", amount, resource.type.name, source.name, shipController.CargoController.Quantity(resource.type));
                    onMinedResources?.Invoke(amount);
                }
                yield return new WaitForSeconds(batchDuration);
            }

            currentMinedSource = null;
            isMining = false;

            StopMining();
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

        Color sensorCoverageGizmoColor = new Color(0, 0, 255, 50);
        private bool isMining;
        private ResourceSource currentMinedSource;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = sensorCoverageGizmoColor;
            Gizmos.DrawWireSphere(transform.position, maxSensorRange);
        }

        private void Update()
        {
            var target = currentMinedSource
                ? currentMinedSource.transform.position
                : miningBeam.transform.position;
            var distance = Mathf.Lerp(miningBeam.transform.localScale.y, Vector3.Distance(
                miningBeam.transform.position, target) / 2.0f, Time.deltaTime);
            miningBeam.transform.localScale = new Vector3(1, distance, 1);

            miningBeamVisualization.SetActive(miningBeam.transform.localScale.y > .0001f);
        }
    }
}
