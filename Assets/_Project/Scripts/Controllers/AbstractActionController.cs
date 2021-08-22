using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Data.UniverseScope;

namespace DoubTech.OpenPath.Controllers
{
    public abstract class AbstractActionController : MonoBehaviour
    {
        public enum State { Inactive, Preparing, InProgress }

        [SerializeField, Tooltip("The range a player needs to be from a source in order to be able to use sensors to detect available resources.")]
        internal float maxSensorRange = 5000;
        [SerializeField, Tooltip("The maximum range for carrying out any interaction controlled by this controller.")]
        float maxInteractionRange = 150;

        internal ShipController shipController;

        internal AudioSource audioSource;

        public State Status { get; set; }

        /// <summary>
        /// Is this an AI controlled ship?
        /// </summary>
        public bool isAI { get; private set; }

        internal virtual void Start()
        {
            audioSource = GetComponent<AudioSource>();
            shipController = GetComponent<ShipController>();
            isAI = shipController is AIShipController;
            Status = State.Inactive;
        }

        /// <summary>
        /// Get the currently available credits to this ontroller.
        /// </summary>
        public virtual float Credits { 
            get => shipController.Credits; 
            set => shipController.Credits = value; 
        }

        /// <summary>
        /// A string describing the current status of this controller.
        /// </summary>
        /// <returns>A string describing the current status of this controller.</returns>
        public abstract string StatusAsString();

        /// <summary>
        /// Test to see if this controller is within range of a desired position.
        /// The range is defined by the `maxInteractionRange` property.
        /// </summary>
        /// <param name="position">The desired position.</param>
        /// <returns>True if within range of the position, otherwise false.</returns>
        protected bool InPosition(Vector3 position)
        {
            return Vector3.SqrMagnitude(position - transform.position) <= maxInteractionRange * maxInteractionRange;
        }

        /// <summary>
        /// Use scanners to scan for planets of a given type.
        /// </summary>
        /// <typeparam name="T">A type identifier for the desired planets.</typeparam>
        /// <returns>A list of all planets that are detected/</returns>
        internal List<T> ScanForObjectsOfType<T>()
        {
            //OPTIMIZATION put planet on its own layer and filter the scan
            int maxColliders = 100;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, maxSensorRange, hitColliders);
            List<T> candidates = new List<T>();
            for (int i = 0; i < numColliders; i++)
            {
                candidates.AddRange(hitColliders[i].GetComponents<T>());
            }

            return candidates;
        }
    }
}
