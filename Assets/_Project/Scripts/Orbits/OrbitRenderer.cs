/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DoubTech.OpenPath.Orbits
{
    [ExecuteAlways]
    [RequireComponent(typeof(LineRenderer))]
    public class OrbitRenderer : MonoBehaviour
    {
        [SerializeField] private int segments = 64;
        [SerializeField] private Orbit orbit;
        [SerializeField] private float width = 10;

        [HideInInspector] [SerializeField] private LineRenderer lineRenderer;

        private Camera camera;

        private void OnValidate()
        {
            if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
            if (!orbit) orbit = GetComponent<Orbit>();

            if (!orbit) return;

            RefreshOrbit();
        }

        public void RefreshOrbit()
        {
            string pts = "";
            lineRenderer.positionCount = segments + 1;
            var positions = new Vector3[segments + 1];
            for (int i = 0; i <= segments; i++)
            {
                positions[i] =
                    orbit.transform.TransformPoint(orbit.ellipse.Evaluate(i / (float) segments));
                pts += positions[i];
            }

            lineRenderer.SetPositions(positions);
        }

        private void OnEnable()
        {
            camera = Camera.main;
        }


        private void Update()
        {
            UpdateLineWidth();

            if (Application.isPlaying & orbit != null)
            {
                orbit.orbitingObjectContainer.transform.RotateAround(
                    orbit.orbitingObjectContainer.transform.position, Vector3.forward,
                    Time.deltaTime * orbit.speed * 2);
            }
        }

        private void UpdateLineWidth()
        {
            if (!camera)
            {
                camera = Camera.current;
                if (!camera)
                {
                    camera = Camera.main;
                }
            }

            #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                camera = SceneView.lastActiveSceneView.camera;
            }
            #endif

            if (camera)
            {
                if (camera.orthographic)
                {
                    var linewidth = (camera.orthographicSize / 10000) * width;
                    lineRenderer.startWidth = linewidth;
                    lineRenderer.endWidth = linewidth;
                }
                else
                {
                    var linewidth = (Vector3.Distance(camera.transform.position, transform.position) / 10000) * width;
                    lineRenderer.startWidth = linewidth;
                    lineRenderer.endWidth = linewidth;
                }
            }
        }
    }
}
