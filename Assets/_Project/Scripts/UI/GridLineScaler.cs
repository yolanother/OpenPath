/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using Lean.Touch;
using UnityEngine;

namespace DoubTech.OpenPath.UI
{
    public class GridLineScaler : MonoBehaviour
    {
        private Renderer renderer;
        private Camera camera;
        private LeanPinchCamera dragCamera;

        private void Start()
        {
            camera = Camera.main;
            renderer = GetComponent<Renderer>();
            dragCamera = camera.GetComponent<LeanPinchCamera>();
        }

        private void Update()
        {
            var clamp = dragCamera.ClampMax - dragCamera.ClampMin;
            var zoom = 1 - (dragCamera.Zoom - dragCamera.ClampMin) / clamp;
            renderer.material.SetFloat("LineThickness", .95f + .045f * zoom);
        }
    }
}
