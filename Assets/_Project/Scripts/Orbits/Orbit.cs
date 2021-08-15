/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using Sirenix.OdinInspector;
using UnityEngine;

namespace DoubTech.OpenPath.Orbits
{
    public class Orbit : MonoBehaviour
    {
        [SerializeField] public Transform orbitingObjectContainer;
        [SerializeField] public Ellipse ellipse;
        [SerializeField] public float speed;
        [Range(0, 1)] [SerializeField] public float startPosition;
        [SerializeField] public bool lerp;
        [SerializeField] public float lerpSpeed;
        [SerializeField] public bool rotateTowardsOrbit;
        [SerializeField] public bool updateDirectionTargetPosition;
        [SerializeField] public Transform directionTarget;

        [HideInInspector] [SerializeField] private OrbitRenderer orbitRenderer;

        private float position = 0;

        public void OnValidate()
        {
            RefreshOrbits();
        }

        public void Update()
        {
            position += Application.isPlaying ? Time.smoothDeltaTime * speed / 1000f : 0;
            if (position > 1) position -= 1;
            if (orbitingObjectContainer)
            {
                SetOrbitPosition(orbitingObjectContainer, position + startPosition);

                if (updateDirectionTargetPosition)
                {
                    SetOrbitPosition(directionTarget, position + startPosition + .01f);
                }

                if (rotateTowardsOrbit)
                {
                    if (updateDirectionTargetPosition)
                    {
                        orbitingObjectContainer.LookAt(directionTarget, transform.up);
                    }
                    else
                    {

                        var target = ellipse.Evaluate(position + .01f + startPosition);
                        orbitingObjectContainer.rotation = Quaternion.Slerp(
                            orbitingObjectContainer.rotation,
                            Quaternion.LookRotation(target, transform.up), Time.deltaTime);
                    }
                }
            }
        }

        private void SetOrbitPosition(Transform orbitingObjectContainer, float t)
        {
            var parent = orbitingObjectContainer.transform.parent;
            var orbitPosition = ellipse.Evaluate(t);
            orbitingObjectContainer.transform.parent = transform;
            if (lerp)
            {
                orbitingObjectContainer.localPosition = Vector3.Lerp(
                    orbitingObjectContainer.localPosition, orbitPosition,
                    Time.deltaTime * lerpSpeed);
            }
            else
            {
                orbitingObjectContainer.localPosition = orbitPosition;
            }

            orbitingObjectContainer.transform.parent = parent;
        }

        [Button]
        public void RefreshOrbits()
        {
            if (!orbitRenderer) orbitRenderer = GetComponentInChildren<OrbitRenderer>();

            if (!orbitingObjectContainer)
            {
                var modelTransform = transform.Find("Planet Model");
                if (modelTransform.childCount > 0)
                {
                    orbitingObjectContainer = modelTransform.GetChild(0);
                }
            }

            Update();
            if (orbitRenderer) orbitRenderer.RefreshOrbit();
        }

        public bool WithinOrbit(Vector3 positionTargetPosition)
        {
            var distance = Vector3.Distance(positionTargetPosition, orbitingObjectContainer.position);
            return distance < ellipse.radiusX && distance < ellipse.radiusY;
        }
    }
}
