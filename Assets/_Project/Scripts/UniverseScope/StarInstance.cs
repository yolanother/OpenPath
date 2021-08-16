/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Data.SolarSystemScope;
using Lean.Common;
using UnityEngine;

namespace DoubTech.OpenPath.UniverseScope
{
    public class StarInstance : MonoBehaviour
    {
        [SerializeField]
        private StarConfig starConfig;
        public StarData starData = new StarData();

        [SerializeField] private LeanSelectableRendererColor selectorColor;

        public StarConfig StarConfig
        {
            get => starConfig;
            set
            {
                starConfig = value;

                star.material.SetColor("_StarColor1", starConfig.starColor);
                float h, s, v;
                Color.RGBToHSV(starConfig.starColor, out h, out s, out v);
                v *= .9f;
                atmosphereClose.material.SetColor("_Color0", Color.HSVToRGB(h, s, v));
                v *= .9f;
                atmosphereFar.material.SetColor("_Color0", Color.HSVToRGB(h, s, v));
                corona.material.SetColor("_coronacolor", starConfig.starColor * starConfig.hdrMultiplier);
                selectorColor.SelectedColor = starConfig.starColor;
            }
        }

        [SerializeField] private Renderer star;
        [SerializeField] private Renderer corona;
        [SerializeField] private Renderer atmosphereClose;
        [SerializeField] private Renderer atmosphereFar;

        private void Start()
        {
            if (starConfig) StarConfig = starConfig;
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                StarConfig = starConfig;
            }
        }
    }
}
