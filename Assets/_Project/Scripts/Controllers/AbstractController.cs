using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DoubTech.OpenPath.Controllers
{
    public abstract class AbstractController : MonoBehaviour
    {
        [SerializeField, Tooltip("The maximum range for carrying out any interaction controlled by this controller.")]
        float maxInteractionRange = 150;

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
    }
}
