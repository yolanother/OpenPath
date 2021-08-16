/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using UnityEngine;

namespace DoubTech.OpenPath
{
    public class CameraRotator : MonoBehaviour
    {
        private Camera camera;

        private void OnEnable()
        {
            camera = Camera.main;
        }

        private void Update()
        {
            switch (Screen.orientation)
            {
                case ScreenOrientation.Portrait:
                case ScreenOrientation.PortraitUpsideDown:
                    camera.transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
                default:
                    camera.transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
            }
        }
    }
}
