/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Data;
using DoubTech.OpenPath.Data.Config;
using DoubTech.OpenPath.Data.Factions;
using DoubTech.OpenPath.Data.Resources;
using DoubTech.OpenPath.Data.SolarSystemScope;
using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UniverseScope;
using DoubTech.OpenPath.UniverseScope.Resources;
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
        [Header("UI Fields")]
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI populatedPlanetsText;
        [SerializeField] private TextMeshProUGUI unpopulatedPlanetsText;
        [SerializeField] private TextMeshProUGUI factionsPresentText;
        [SerializeField] private TextMeshProUGUI resourceDemandText;
        [SerializeField] private TextMeshProUGUI resourceSupplyText;

        [Header("UI Images")]
        [SerializeField] private Image bgOverlay;
        [SerializeField] private Image bg;
        [SerializeField] private Image headerBg;
        [SerializeField] private Image headerOverlay;

        [Header("Events")]
        [SerializeField] private StarSelectionGameEvent onDeselectStar;
        [SerializeField] private Vector2GameEvent onTravelToSolarSystemEvent;

        private UIGradient[] buttons;
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
            Dictionary<Faction, int> factionsPresent = new Dictionary<Faction, int>(); // key = faction, value = # of planets owned
            Dictionary<ProductionResource, float> supply = new Dictionary<ProductionResource, float>(); // key = resource, value = quantity demanded
            Dictionary<ProductionResource, float> demand = new Dictionary<ProductionResource, float>(); // key = resource, value = quantity demanded

            Vector2 starCoordinates = starInstance.starData.Coordinates;
            
            SolarSystemConfig systemConfig = GameManager.Instance.galaxyConfig.solarSystemConfig;
            float[] planetDistances = systemConfig.GetPlanetPositions(starCoordinates);
            for (int i = 0; i < planetDistances.Length; i++)
            {
                PlanetConfig config = systemConfig.GetPlanetConfig(starCoordinates, i, planetDistances[i]);
                PlanetInstance planetInstance= GameManager.Instance.solarSystemInstance.CreatePlanetInstance(starCoordinates, i, config, planetDistances[i]);
                PlanetData planetData = planetInstance.planetData;

                // Count of planets
                if (planetData.Population > 0)
                {
                    inhabitedCount++;
                } else
                {
                    uninhabitedCount++;
                }

                // Get Faction Data
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

                // Resources for Trade
                ResourceDemand[] trades = planetInstance.GetComponents<ResourceDemand>();
                for (int b = 0; b < trades.Length; b++)
                {
                    float quantity;
                    if (demand.TryGetValue(trades[b].resource, out quantity))
                    {
                        demand[trades[b].resource] = trades[b].requiredQuantity + quantity;
                    } else
                    {
                        demand.Add(trades[b].resource, trades[b].requiredQuantity);
                    }
                }

                ResourceSource[] mines = planetInstance.GetComponents<ResourceSource>();
                for (int b = 0; b < mines.Length; b++)
                {
                    float quantity;
                    if (supply.TryGetValue(mines[b].resource, out quantity))
                    {
                        supply[mines[b].resource] = mines[b].resourceAvailable + quantity;
                    } else
                    {
                        supply.Add(mines[b].resource, mines[b].resourceAvailable);
                    }
                }
            }

            title.text = starInstance.starData.DisplayName;
            populatedPlanetsText.text = $"Populated Planets: {inhabitedCount}";
            unpopulatedPlanetsText.text = $"Unpopulated Planets: {uninhabitedCount}";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < factionsPresent.Count; i++)
            {
                string name = factionsPresent.ElementAt(i).Key.name;
                int count = factionsPresent.ElementAt(i).Value;
                sb.AppendLine($"{name}: {count} planets.");
            }
            factionsPresentText.text = sb.ToString();

            sb = new StringBuilder("Resources in demand:\n");
            if (demand.Count == 0)
            {
                sb.AppendLine("None");
            }
            else
            {
                for (int i = 0; i < demand.Count; i++)
                {
                    string name = demand.ElementAt(i).Key.name;
                    float quantity = demand.ElementAt(i).Value;
                    sb.AppendLine($"{quantity} of {name}.");
                }
            }
            resourceDemandText.text = sb.ToString();

            sb = new StringBuilder("Resources to be mined:\n");
            if (supply.Count == 0)
            {
                sb.AppendLine("None");
            }
            else
            {
                for (int i = 0; i < supply.Count; i++)
                {
                    string name = supply.ElementAt(i).Key.name;
                    float quantity = supply.ElementAt(i).Value;
                    sb.AppendLine($"{quantity} of {name}.");
                }
            }
            resourceSupplyText.text = sb.ToString();
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
