/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.UniverseScope;
using DoubTech.ScriptableEvents.BuiltinTypes;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubTech.OpenPath.UI
{
    public class StarHud : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;

        [SerializeField] private Image bgOverlay;
        [SerializeField] private Image bg;
        [SerializeField] private Image headerBg;
        [SerializeField] private Image headerOverlay;

        private UIGradient[] buttons;


        [SerializeField] private StarSelectionGameEvent onDeselectStar;
        [SerializeField] private Vector2GameEvent onTravelToSolarSystemEvent;

        private StarInstance starInstance;

        public StarInstance StarInstance
        {
            get => starInstance;
            set
            {
                starInstance = value;
                title.text = value.starData.DisplayName;
                bgOverlay.color = starInstance.StarConfig.starColor;
                headerOverlay.color = starInstance.StarConfig.starColor;
                var alpha = starInstance.StarConfig.starColor;
                alpha.a = .5f;
                bg.color = alpha;
                headerBg.color = alpha;

                if(null == buttons) buttons = GetComponentsInChildren<UIGradient>();

                float h, s, v;
                Color.RGBToHSV(starInstance.StarConfig.starColor, out h, out s, out v);
                var leftColor = Color.HSVToRGB(h, s, v * .75f);
                var rightColor = Color.HSVToRGB(h, s, v * .85f);
                foreach (UIGradient button in buttons)
                {
                    var colorKeys = button.EffectGradient.colorKeys;
                    var alphaKeys = button.EffectGradient.alphaKeys;
                    colorKeys[0].color = leftColor;
                    colorKeys[1].color = rightColor;
                    button.EffectGradient.SetKeys(colorKeys, alphaKeys);
                }

            }
        }

        public void Travel()
        {
            onTravelToSolarSystemEvent.Invoke(starInstance.starData.Coordinates);
        }

        public void DeselectPlanet()
        {
            onDeselectStar?.Invoke(starInstance);
        }
    }
}
