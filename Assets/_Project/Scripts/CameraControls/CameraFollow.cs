/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Orbits;
using DoubTech.ScriptableEvents;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.OpenPath.CameraControls
{
    [Serializable]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] public Transform target;
        [SerializeField] private Camera camera;
        [SerializeField] private LeanPinchCamera pinchCamera;
        [SerializeField] private LeanDragCamera dragCamera;

        [Header("Speeds")]
        [SerializeField] private float transformSpeed = 4;
        [SerializeField] private float zoomSpeed = 4;

        [Header("Offsets")]
        [SerializeField] Vector2 offset = Vector2.zero;
        [SerializeField] private float orthographicSize = 1000;
        private float preTargetZoom;
        private Vector3 preTargetPosition;
        private bool hasPreviousPositionData;

        public Transform Target
        {
            get => target;
            set
            {
                if (!hasPreviousPositionData)
                {
                    preTargetZoom = pinchCamera.Zoom;
                    preTargetPosition = pinchCamera.transform.position;
                    hasPreviousPositionData = true;
                }

                target = value;
                var orbit = target.GetComponentInChildren<Orbit>();
                if (orbit)
                {
                    target = orbit.orbitingObjectContainer;
                }

                if (!target)
                {

                }
            }
        }

        private void OnEnable()
        {
            if (!camera)
            {
                camera = Camera.main;
            }
        }

        private void Update()
        {
            if (target)
            {
                var targetPosition = new Vector3(
                    target.transform.position.x + offset.x,
                    target.transform.position.y + offset.y,
                    camera.transform.position.z);
                dragCamera.enabled = false;
                camera.transform.position = Vector3.Lerp(
                    camera.transform.position,
                    targetPosition,
                    Time.deltaTime * transformSpeed);

                pinchCamera.Zoom = Mathf.Lerp(pinchCamera.Zoom, orthographicSize,
                    Time.deltaTime * zoomSpeed);

                if (Vector3.Distance(camera.transform.position, targetPosition) < .001f)
                {
                    camera.transform.position = targetPosition;
                }
            }
            else if (hasPreviousPositionData)
            {
                camera.transform.position = Vector3.Lerp(camera.transform.position,
                    preTargetPosition,
                    Time.deltaTime * transformSpeed);

                pinchCamera.Zoom = Mathf.Lerp(pinchCamera.Zoom, preTargetZoom,
                    Time.deltaTime * zoomSpeed);

                if (Vector3.Distance(camera.transform.position, preTargetPosition) < .05f)
                {
                    hasPreviousPositionData = false;
                }
            }
            else
            {
                dragCamera.enabled = true;
            }
        }

        public void ClearSelection()
        {
            target = null;
        }
    }
}
