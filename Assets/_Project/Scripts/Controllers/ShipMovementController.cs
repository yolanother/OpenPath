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
using DoubTech.OpenPath.Data.UniverseScope;
using DoubTech.OpenPath.Events;

namespace DoubTech.OpenPath.Controllers
{
    /// <summary>
    /// Ship Movement Controller is responsible for moving the ship through space.
    /// </summary>
    public class ShipMovementController : AbstractActionController
    {
        [SerializeField] private Transform positionTarget;
        [SerializeField] private Transform lookTarget;
        [SerializeField] private Orbit orbit;
        [SerializeField] private Camera camera;
        [SerializeField] private Transform modelPivot;

        [SerializeField] private float velocity = 15f;
        [SerializeField] private float acceleration = 1;
        [SerializeField] private float stopDistance = .5f;

        [SerializeField] public ShipOrbitGameEvent onEnteredOrbit;
        [SerializeField] public ShipOrbitGameEvent onLeftOrbit;

        private Transform orbitTarget;

        private float currentSpeed;
        private Vector3 lastPosition;
        private Quaternion nextAngle;
        private bool movingIntoOrbit;

        private PlanetInstance orbitPlanetTarget;
        private bool lockLookTarget;
        private Vector3 lockLookPosition;

        /// <summary>
        /// The transform that this ship is currently targeting for orbit.
        ///
        /// This can be anything that supports orbits. Ex: Planet, Star
        /// </summary>
        public Transform OrbitTarget => orbitTarget;

        /// <summary>
        /// Returns the targeted planet that the ship is attempting to orbit
        /// </summary>
        public PlanetInstance OrbitPlanetTarget => orbitPlanetTarget;

        /// <summary>
        /// Get the position of the current destination.
        /// </summary>
        public Vector3 currentDestination { get => positionTarget.position; }

        internal override void Start()
        {
            base.Start();
            // Move components that will be moved independently out of the main transform
            orbit.transform.parent = null;
            lookTarget.transform.parent = null;
            positionTarget.transform.parent = null;

            camera = Camera.main;
        }

        private void OnDestroy()
        {
            Destroy(orbit);
        }

        /// <summary>
        /// Move the ship to a given position and rotation.
        /// </summary>
        /// <param name="target">The position to move to.</param>
        public void MoveToOrbit(Transform target)
        {
            orbitTarget = target;
            orbitPlanetTarget = orbitTarget.GetComponent<PlanetInstance>();

            Debug.LogFormat($"{gameObject.name} is moving to orbit around {orbitTarget.gameObject.name}");
        }

        public void MoveToOrbit(InvestmentOpportunity opportunity)
        {
            MoveToOrbit(((Component)opportunity).transform);
        }

        public void MoveToOrbit(ResourceSource source)
        {
            MoveToOrbit(((Component)source).transform);
        }

        public void MoveToOrbit(ResourceDemand demand)
        {
            MoveToOrbit(((Component)demand).transform);
        }

        public void MoveToOrbit(EquipmentTrade offer)
        {
            MoveToOrbit(((Component)offer).transform);
        }

        public void MoveToOrbit(PlanetInstance planet)
        {
            MoveToOrbit(planet.transform);
        }

        private void Update()
        {
            if (orbitTarget)
            {
                orbit.transform.position = orbitTarget.position;
                positionTarget.position = orbit.transform.position;
            }
            if (orbitTarget && orbit.WithinOrbit(orbitTarget.position))
            {
                orbit.gameObject.SetActive(true);
                orbit.RefreshOrbits();
                modelPivot.LookAt(orbit.transform);

                if (movingIntoOrbit && orbit.WithinOrbit(transform.position))
                {
                    movingIntoOrbit = false;
                    var planetInstance = orbitTarget.GetComponent<PlanetInstance>();
                    planetInstance.AddOrbitingShip(shipController);
                    if (planetInstance)
                    {
                        onEnteredOrbit?.Invoke(shipController, planetInstance);
                    }
                }
            }
            else
            {
                modelPivot.localEulerAngles = new Vector3(180, 90, 150);
                orbit.gameObject.SetActive(false);
                var distance = Vector3.Distance(transform.position, positionTarget.position);
                if (distance > stopDistance)
                {
                    var direction = (positionTarget.position - transform.position).normalized;

                    transform.position += direction * Mathf.Min(distance, currentSpeed);
                    lookTarget.position = positionTarget.position;

                    var accelDirection = distance > stopDistance * velocity ? 1 : -1;
                    currentSpeed = Mathf.Clamp(velocity + acceleration * accelDirection, 0,
                        velocity);
                }
            }

            var angle = transform.rotation;
            Vector3 lookPosition = transform.position;
            if (lastPosition != transform.position)
            {
                lookPosition = transform.position + (transform.position - lastPosition);
                if (orbitTarget)
                {
                    lookPosition = transform.position + (orbit.NextOrbitPosition - orbit.PreviousOrbitPosition);
                }

                if (lockLookTarget)
                {
                    lookPosition = lockLookPosition;
                }
                transform.LookAt(lookPosition);
                nextAngle = transform.rotation;
            }

            transform.rotation = Quaternion.Slerp(
                angle,
                nextAngle,
                Time.deltaTime * 4);
            Debug.DrawLine(transform.position, lookPosition + (lookPosition - transform.position) * 1000);

            lastPosition = transform.position;
        }

        private void OnDrawGizmos()
        {
            var target = transform.position + (transform.position - lastPosition);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, .25f);
            Gizmos.DrawSphere(transform.position + target * 4, .25f);
            Gizmos.DrawLine(transform.position, target);
        }

        public override string StatusAsString()
        {
            if (orbitTarget)
            {
                return "Moving to, or currently in, orbit around " + orbitTarget.name;
            } else
            {
                return "No orbit target set.";
            }
        }

        public void Orbit(PlanetInstance planetInstance)
        {
            if (planetInstance)
            {
                movingIntoOrbit = true;
                if (orbitTarget && planetInstance.transform != orbitTarget)
                {
                    var oldPlanet = orbitTarget.GetComponent<PlanetInstance>();
                    if(oldPlanet) oldPlanet.RemoveOrbitingObject(shipController);
                }
                orbitTarget = planetInstance.transform;
                orbitPlanetTarget = planetInstance;
            }
            else
            {
                OnLeaveOrbit();
            }
        }

        public void LeaveOrbit()
        {
            movingIntoOrbit = false;
            positionTarget.position = transform.position;
            OnLeaveOrbit();
        }

        private void OnLeaveOrbit()
        {
            if (orbitTarget)
            {
                var planetInstance = orbitTarget.GetComponent<PlanetInstance>();
                planetInstance.RemoveOrbitingObject(shipController);
                onLeftOrbit.Invoke(shipController, planetInstance);
                orbitPlanetTarget = null;
            }

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
            if (lockLookTarget)
            {
                lockLookPosition = worldPos;
            }
            else
            {
                movingIntoOrbit = true;
                var pos = new Vector3(worldPos.x, worldPos.y);
                if (orbitTarget && !orbit.WithinOrbit(pos))
                {
                    orbitTarget = null;
                    orbitPlanetTarget = null;
                }

                positionTarget.position = pos;
            }
        }

        public void LockLook()
        {
            lockLookTarget = true;
        }

        public void UnlockLook()
        {
            lockLookTarget = false;
        }

        /// <summary>
        /// Stop the current move order. This will result in the ship stopping moving and holding its current position.
        /// </summary>
        public void Stop()
        {
            MoveTo(transform.position);
        }
    }
}
