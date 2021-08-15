using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UniverseScope;
using DoubTech.OpenPath.UniverseScope.Resources;
using System;
using DoubTech.OpenPath.UniverseScope.Equipment;
using DoubTech.OpenPath.Orbits;
using Lean.Touch;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// Ship Movement Controller is responsible for moving the ship through space.
    /// </summary>
    public class ShipMovementController : AbstractController
    {
        [SerializeField] private Transform positionTarget;
        [SerializeField] private Transform lookTarget;
        [SerializeField] private Orbit orbit;
        [SerializeField] private Camera camera;

        [SerializeField] private float velocity = 15f;
        [SerializeField] private float acceleration = 1;
        [SerializeField] private float stopDistance = .5f;

        private Transform orbitTarget;
        private float orbitingDistance = 1.5f;

        private float orbitingDistanceSqr;

        private float currentSpeed;

        private void Start()
        {
            // Move components that will be moved independently out of the main transform
            orbit.transform.parent = null;
            lookTarget.transform.parent = null;
            positionTarget.transform.parent = null;

            orbitingDistanceSqr = orbitingDistance * orbitingDistance;
            camera = Camera.main;
        }

        public bool InPosition
        {
            get
            {
                if (orbitTarget == null) return true;

                return Mathf.Approximately(orbitingDistanceSqr, Vector3.SqrMagnitude(orbitTarget.position - transform.position));
            }
        }

        /// <summary>
        /// Move the ship to a given position and rotation.
        /// </summary>
        /// <param name="transform">The position to move to.</param>
        /// <param name="distance">The distance to maintain from the position.</param>
        public void MoveToOrbit(Transform transform, float distance)
        {
            orbitTarget = transform;
            orbit.ellipse.radiusX = distance;
            orbit.ellipse.radiusY = distance;
            orbitingDistanceSqr = orbitingDistance * orbitingDistance;
            Debug.LogWarning("TODO: Animate move to orbit. For now we just teleport there.");

            Debug.LogFormat("Moved to orbit around {0}.", transform.gameObject.name);
        }

        public void MoveToOrbit(ResourceSource source, float distance)
        {
            MoveToOrbit(((Component)source).transform, distance);
        }

        public void MoveToOrbit(ResourceDemand demand, float distance)
        {
            MoveToOrbit(((Component)demand).transform, distance);
        }

        public void MoveToOrbit(EquipmentOffer offer, float distance)
        {
            MoveToOrbit(((Component)offer).transform, distance);
        }

        public void MoveToOrbit(PlanetInstance planet, float distance)
        {
            MoveToOrbit(planet.transform, distance);
        }

        private void Update()
        {
            transform.LookAt(lookTarget, transform.up);

            if (orbitTarget)
            {
                orbit.gameObject.SetActive(true);
                orbit.directionTarget = lookTarget;
                orbit.updateDirectionTargetPosition = true;
                orbit.transform.position = orbitTarget.position;
                orbit.RefreshOrbits();
            }
            else
            {
                orbit.gameObject.SetActive(false);
                var distance = Vector3.Distance(transform.position, positionTarget.position);
                if (distance > stopDistance)
                {
                    var direction = (positionTarget.position - transform.position).normalized;

                    transform.position += direction * Mathf.Min(distance, currentSpeed);
                    lookTarget.position = positionTarget.position;

                    if (distance > velocity)
                    {
                        transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            Quaternion.LookRotation(lookTarget.position, transform.up),
                            Time.deltaTime);
                    }

                    var accelDirection = distance > stopDistance * velocity ? 1 : -1;
                    currentSpeed = Mathf.Clamp(velocity + acceleration * accelDirection, 0,
                        velocity);
                }
            }
        }

        public override string StatusAsString()
        {
            if (InPosition)
            {
                return "Currently in position.";
            } else
            {
                return "Not in position.";
            }
        }

        public void Orbit(PlanetInstance planetInstance)
        {
            if (planetInstance)
            {
                orbitTarget = planetInstance.transform;
            }
            else
            {
                orbitTarget = null;
            }
        }

        public void LeaveOrbit()
        {
            orbitTarget = null;
        }

        public void OnEnteredGravitationalField(Collider other)
        {
            var planetInstance = other.GetComponentInParent<PlanetInstance>();
            if (planetInstance && planetInstance.orbit.WithinOrbit(positionTarget.position))
            {
                Orbit(planetInstance);
            }
        }

        public void MoveTo(Vector3 worldPos)
        {
            var pos = new Vector3(worldPos.x, worldPos.y);
            if (orbitTarget && !orbit.WithinOrbit(pos))
            {
                orbitTarget = null;
            }

            positionTarget.position = pos;
        }
    }
}
