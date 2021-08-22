/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DoubTech.OpenPath.CameraControls
{
    public class CameraZoomObjectScaler : MonoBehaviour
    {
        [SerializeField] private Camera trackingCamera;

        [SerializeField] private Vector2 objectSizeRange = new Vector2(.25f, 10);
        [SerializeField] private Vector2 cameraZoomRange = new Vector2(0, 10000);

        private void Start()
        {
            if (!trackingCamera) trackingCamera = Camera.main;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!trackingCamera) trackingCamera = Camera.main;
        }

        private void Update()
        {
            var cameraPercentage = (trackingCamera.orthographicSize - cameraZoomRange.x) /
                                   (cameraZoomRange.y - cameraZoomRange.x);

            var scale = Mathf.Lerp(objectSizeRange.x, objectSizeRange.y, cameraPercentage);
            transform.localScale = scale * Vector3.one;
        }
    }
}
