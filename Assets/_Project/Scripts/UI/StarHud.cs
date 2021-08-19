/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Data;
using DoubTech.OpenPath.Data.Config;
using DoubTech.OpenPath.Data.Factions;
using DoubTech.OpenPath.Data.SolarSystemScope;
using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.UniverseScope;
using DoubTech.ScriptableEvents.BuiltinTypes;
using Michsky.UI.ModernUIPack;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubTech.OpenPath.UI
{
    public class StarHud : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI populatedPlanetsText;
        [SerializeField] private TextMeshProUGUI unpopulatedPlanetsText;
        [SerializeField] private TextMeshProUGUI factionsPresentText;

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
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            SetUIColour();
            SetUIData();
        }

        private void SetUIData()
        {
            int inhabitedCount = 0;
            int uninhabitedCount = 0;
            Dictionary<Faction, int> factionsPresent = new Dictionary<Faction, int>();

            Vector2 starCoordinates = starInstance.starData.Coordinates;
            
            SolarSystemConfig systemConfig = GameManager.Instance.galaxyConfig.solarSystemConfig;
            float[] planetDistances = systemConfig.GetPlanetPositions(starCoordinates);
            for (int i = 0; i < planetDistances.Length; i++)
            {
                PlanetConfig config = systemConfig.GetPlanetConfig(starCoordinates, i, planetDistances[i]);
                PlanetData planetData = GameManager.Instance.solarSystemInstance.CreatePlanetInstance(starCoordinates, i, config, planetDistances[i]).planetData;
                if (planetData.population > 0)
                {
                    inhabitedCount++;
                } else
                {
                    uninhabitedCount++;
                }

                if (planetData.faction != null)
                {
                    int count;
                    if (factionsPresent.TryGetValue(planetData.faction, out count))
                    {
                        factionsPresent[planetData.faction] = count++;
                    } else
                    {
                        count = 1;
                        factionsPresent.Add(planetData.faction, count);
                    }
                }
            }

            title.text = starInstance.starData.DisplayName;
            populatedPlanetsText.text = $"Populated Planets: {inhabitedCount}";
            unpopulatedPlanetsText.text = $"Unpopulated Planets: {uninhabitedCount}";

            StringBuilder factionsText = new StringBuilder();
            for (int i = 0; i < factionsPresent.Count; i++)
            {
                string name = factionsPresent.ElementAt(i).Key.name;
                int count = factionsPresent.ElementAt(i).Value;
                factionsText.AppendLine($"{name}: {count} planets.");
            }
            factionsPresentText.text = factionsText.ToString();
        }

        private void SetUIColour()
        {
            bgOverlay.color = starInstance.StarConfig.starColor;
            headerOverlay.color = starInstance.StarConfig.starColor;
            var alpha = starInstance.StarConfig.starColor;
            alpha.a = .5f;
            bg.color = alpha;
            headerBg.color = alpha;

            if (null == buttons) buttons = GetComponentsInChildren<UIGradient>();

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
