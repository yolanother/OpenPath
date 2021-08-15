/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using UnityEngine;

namespace DoubTech.OpenPath.Orbits
{
    public class Orbit : MonoBehaviour
    {
        [SerializeField] public Transform orbitingObjectContainer;
        [SerializeField] public Ellipse ellipse;
        [SerializeField] public float speed;
        [Range(0, 1)] [SerializeField] public float startPosition;

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
                orbitingObjectContainer.localPosition =
                    ellipse.Evaluate(position + startPosition);
            }
        }

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
    }
}
