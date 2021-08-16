/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using System.Linq;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UniverseScope;
using DoubTech.OpenPath.UniverseScope.Equipment;
using DoubTech.OpenPath.UniverseScope.Resources;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubTech.OpenPath.UI
{
    public class PlanetHud : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;

        [SerializeField] private PlanetSelectionGameEvent onVisitPlanet;
        [SerializeField] private PlanetSelectionGameEvent onLeavePlanetEvent;
        [SerializeField] private PlanetSelectionGameEvent onDeselectPlanet;

        [SerializeField] private Image bgOverlay;
        [SerializeField] private Image bg;
        [SerializeField] private Image headerBg;
        [SerializeField] private Image headerOverlay;

        [SerializeField] private TextMeshProUGUI population;
        [SerializeField] private TextMeshProUGUI habitibility;
        [SerializeField] private TextMeshProUGUI mineableResources;

        [SerializeField] private ButtonManagerBasic visitButton;
        [SerializeField] private ButtonManagerBasic actionButtonMine;
        [SerializeField] private ButtonManagerBasic actionButtonNuke;
        [SerializeField] private ButtonManagerBasic actionButtonTrade;
        [SerializeField] private ButtonManagerBasic actionButtonSellAll;

        private UIGradient[] buttons;

        private PlanetInstance planetInstance;
        private ResourceDemand[] resourceDemands;
        private EquipmentTrade[] equipmentTrades;
        private ResourceSource[] resourceSources;

        private ShipController orbitingShip;

        public PlanetInstance PlanetInstance
        {
            get => planetInstance;
            set
            {
                planetInstance = value;
                var planetData = value.planetData;
                title.text = value.planetData.DisplayName;
                bgOverlay.color = planetData.FactionColor;
                headerOverlay.color = planetData.FactionColor;
                var alpha = planetData.FactionColor;
                alpha.a = .5f;
                bg.color = alpha;
                headerBg.color = alpha;

                if (null == buttons) buttons = GetComponentsInChildren<UIGradient>();

                float h, s, v;
                Color.RGBToHSV(planetData.FactionColor, out h, out s, out v);
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

                resourceDemands = planetInstance.GetComponents<ResourceDemand>();
                equipmentTrades = planetInstance.GetComponents<EquipmentTrade>();
                resourceSources = planetInstance.GetComponents<ResourceSource>();

                if (planetInstance.OrbitingShips.Length > 0)
                {
                    orbitingShip = planetInstance.OrbitingShips.First();
                }

                UpdateData();
            }
        }

        private void UpdateData()
        {
            population.text = $"Population: {planetInstance.planetData.Population}";
            habitibility.text = $"Habitibility: {planetInstance.planetData.HabitabilityString}";

            bool inhabited = planetInstance.planetData.Population != 0;
            actionButtonMine.gameObject.SetActive(!inhabited);
            actionButtonNuke.gameObject.SetActive(false);
            actionButtonTrade.gameObject.SetActive(inhabited);
            actionButtonSellAll.gameObject.SetActive(inhabited);

            if (orbitingShip)
            {
                visitButton.buttonText = "DEPART";
                visitButton.UpdateUI();
            }
            else
            {
                visitButton.buttonText = "VISIT";
                visitButton.UpdateUI();
            }

            string mineableSources = "";
            if (resourceSources.Length > 0)
            {
                foreach (var resourceSource in resourceSources)
                {
                    if (mineableSources.Length > 0) mineableSources += "\n";
                    mineableSources +=
                        $"{resourceSource.ResourceType.name}: {resourceSource.resourceAvailable.ToString("F")}";
                }
            }
            else
            {
                mineableSources = "None";
            }

            mineableResources.text = mineableSources;
        }

        public void Visit()
        {
            if (orbitingShip)
            {
                orbitingShip.MovementController.LeaveOrbit();
            }
            else
            {
                onVisitPlanet.Invoke(planetInstance);
            }
        }

        public void DeselectPlanet()
        {
            onDeselectPlanet?.Invoke(planetInstance);
        }

        public void Mine()
        {
            if (orbitingShip && orbitingShip.MiningController && planetInstance.planetData.Population == 0)
            {
                for (int i = 0; i < resourceSources.Length; i++)
                {
                    var resource = resourceSources[i];
                    if (resource.ResourceAvailable)
                    {
                        orbitingShip.MiningController.Mine(resource);
                        break;
                    }
                }
            }
        }

        public void SellAll()
        {
            if (orbitingShip && orbitingShip.TradeController && resourceDemands.Length > 0)
            {
                orbitingShip.TradeController.SellAll(planetInstance);
            }
        }

        public void OnShipEnteredOrbit(ShipController ship, PlanetInstance planet)
        {
            if (planet == planetInstance)
            {
                orbitingShip = ship;
                UpdateData();
            }
        }

        private void Update()
        {
            if (planetInstance && !orbitingShip && planetInstance.HasShipInOrbit)
            {
                orbitingShip = planetInstance.OrbitingShips.First();
                UpdateData();
            }
            else if (planetInstance && orbitingShip && !planetInstance.HasShipInOrbit)
            {
                orbitingShip = null;
                UpdateData();
            }

        }
    }
}
