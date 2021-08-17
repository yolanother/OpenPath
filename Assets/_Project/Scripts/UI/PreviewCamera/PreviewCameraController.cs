/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubTech.OpenPath.UI.PreviewCamera
{
    public class PreviewCameraController : MonoBehaviour
    {
        private static PreviewCameraController instance;
        [SerializeField] private Camera camera;

        public static PreviewCameraController Instance => instance;

        private Queue<ScreenshotTask> screenshotQueue = new Queue<ScreenshotTask>();

        private class ScreenshotTask
        {
            public PreviewCameraTarget target;
            public Action<Sprite> onSpriteReady;
        }

        private void OnEnable()
        {
            instance = this;
        }

        private void OnDisable()
        {
            instance = null;
        }

        public void TakeScreenshot(PreviewCameraTarget target, Action<Sprite> onSpriteReady)
        {
            var task = new ScreenshotTask()
            {
                target = target,
                onSpriteReady = onSpriteReady
            };
            screenshotQueue.Enqueue(task);
            StartCoroutine(TakeScreenshot(task));
        }

        public void TakeScreenshot(GameObject gameObject, Action<Sprite> onSpriteReady)
        {
            var target = gameObject.GetComponentInChildren<PreviewCameraTarget>();
            TakeScreenshot(target, onSpriteReady);
        }

        public void TakeScreenshot(Component component, Action<Sprite> onSpriteReady)
        {
            var target = component.gameObject.GetComponentInChildren<PreviewCameraTarget>();
            TakeScreenshot(target, onSpriteReady);
        }

        private IEnumerator TakeScreenshot(ScreenshotTask task)
        {
            camera.enabled = true;
            while (screenshotQueue.Peek() != task) yield return null;
            transform.parent = task.target.cameraContainer;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            yield return new WaitForEndOfFrame();
            var resWidth = 512;
            var resHeight = 512;

            Texture2D screenshot;

            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = rt;
            screenshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            screenshot.Apply();
            camera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            var sprite = Sprite.Create(screenshot, new Rect(0, 0, resWidth, resHeight),
                new Vector2(0, 0));
            task.onSpriteReady.Invoke(sprite);
            camera.enabled = false;
            screenshotQueue.Dequeue();
        }
    }
}
