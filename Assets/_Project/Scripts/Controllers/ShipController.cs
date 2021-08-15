using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UniverseScope;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// Ship Controller is responsible for moving the ship through space.
    /// </summary>
    public class ShipController : MonoBehaviour
    {
        private Transform objectToOrbit;
        private float orbitingDistance = 1.5f;

        private float orbitingDistanceSqr;

        private void Start()
        {
            orbitingDistanceSqr = orbitingDistance * orbitingDistance;
        }

        public bool InPosition
        {
            get
            {
                if (objectToOrbit == null) return true;

                return Mathf.Approximately(orbitingDistanceSqr, Vector3.SqrMagnitude(objectToOrbit.position - transform.position));
            }
        }

        /// <summary>
        /// Move the ship to a given position and rotation.
        /// </summary>
        /// <param name="transform">The position to move to.</param>
        /// <param name="distance">The distance to maintain from the position.</param>
        public void MoveToOrbit(Transform transform, float distance)
        {
            objectToOrbit = transform;
            orbitingDistance = distance;
            orbitingDistanceSqr = orbitingDistance * orbitingDistance;
            Debug.LogFormat("TODO: move to orbit around {0}. For now we just teleport there.", transform.gameObject.name);
        }

        public void MoveToOrbit(ResourceSource source, float distance)
        {
            MoveToOrbit(((Component)source).transform, distance);
        }

        public void MoveToOrbit(PlanetInstance planet, float distance)
        {
            MoveToOrbit(planet.transform, distance);
        }

        private void Update()
        {
            if (objectToOrbit != null) {
                Vector3 direction = (transform.position - objectToOrbit.position).normalized;
                transform.position = objectToOrbit.position + direction * orbitingDistance; 
            }
        }
    }
}
