/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.OpenPath.UI
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool interactable;
        [SerializeField] private bool blocksRaycasts;
        [SerializeField] private bool visible;
        [SerializeField] private float fadeSpeed = 4;

        [SerializeField] private UnityEvent onVisible = new UnityEvent();
        [SerializeField] private UnityEvent onInvisible = new UnityEvent();

        private bool isVisible;

        public bool Visible
        {
            get => visible;
            set => visible = value;
        }

        private void Start()
        {
            canvasGroup.alpha = visible ? 1 : 0;
            isVisible = visible;
        }

        private void Update()
        {
            if (visible != isVisible)
            {
                canvasGroup.alpha += (visible ? 0.01f : -0.01f) * fadeSpeed;

                if (canvasGroup.alpha <= 0 && isVisible)
                {
                    isVisible = false;
                    onInvisible.Invoke();
                }
                else if (canvasGroup.alpha >= 1 && !isVisible)
                {
                    isVisible = true;
                    onVisible.Invoke();
                }

                canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha);
            }
            canvasGroup.interactable = visible && interactable;
            canvasGroup.blocksRaycasts = visible && blocksRaycasts;
        }
    }
}
