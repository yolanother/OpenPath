/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Orbits;
using DoubTech.OpenPath.SolarSystemScope;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace DoubTech.OpenPath.ShipControls
{
    public class ShipController : MonoBehaviour
    {
        [SerializeField] private Transform positionTarget;
        [SerializeField] private Transform lookTarget;
        [SerializeField] private Transform modelContainer;
        [SerializeField] private Orbit orbit;
        [SerializeField] private Camera camera;

        [SerializeField] private float velocity;

        private Transform orbitTarget;

        private void OnEnable()
        {
            if (!camera)
            {
                camera = Camera.main;
            }
        }

        private void Update()
        {
            modelContainer.LookAt(lookTarget, transform.up);

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
                modelContainer.position = Vector3.Lerp(modelContainer.position, positionTarget.position, Time.deltaTime * velocity);
                lookTarget.position = Vector3.Lerp(lookTarget.position, positionTarget.position, Time.deltaTime);

                modelContainer.rotation = Quaternion.Slerp(
                    modelContainer.rotation,
                    Quaternion.LookRotation(lookTarget.position, transform.up), Time.deltaTime);
            }

            if (Input.GetMouseButtonDown(0))
            {
                var point = camera.ScreenToWorldPoint(Input.mousePosition);
                point.z = 0;
                positionTarget.position = point;
                positionTarget.transform.parent = null;
            }
        }

        public void Orbit(PlanetInstance planetInstance)
        {
            if (planetInstance)
            {
                orbitTarget = planetInstance.planetTransform;
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

        public void LeanTap(LeanFinger finger)
        {
            var worldPos = finger.GetWorldPosition(100000);
            var pos = new Vector3(worldPos.x, worldPos.y);
            if (orbitTarget && !orbit.WithinOrbit(pos))
            {
                orbitTarget = null;
            }

            positionTarget.position = pos;
        }
    }
}
