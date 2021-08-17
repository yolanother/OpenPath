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
using Lean.Common;
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

        [SerializeField] private Image[] tintedUIBackgrounds;
        [SerializeField] private Image[] tintedUIOverlay;

        [SerializeField] private TextMeshProUGUI population;
        [SerializeField] private TextMeshProUGUI habitibility;
        [SerializeField] private TextMeshProUGUI mineableResources;
        [SerializeField] private TextMeshProUGUI desiredResources;
        [SerializeField] private TextMeshProUGUI distance;

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
        private bool orbitingShip;

        public PlanetInstance PlanetInstance
        {
            get => planetInstance;
            set
            {
                planetInstance = value;
                var planetData = value.planetData;
                title.text = value.planetData.DisplayName;
                var alpha = planetData.FactionColor;
                alpha.a = .5f;

                foreach (var bg in tintedUIBackgrounds)
                {
                    bg.color = alpha;
                }

                foreach (var overlay in tintedUIOverlay)
                {
                    overlay.color = planetData.FactionColor;
                }


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

                // TODO: We will need to set this from planet data. Hacking it here for now
                var selectColor = planetInstance.GetComponentInChildren<LeanSelectableRendererColor>();
                selectColor.SelectedColor = planetData.FactionColor;

                UpdateData();
            }
        }

        public void UpdateData()
        {
            if (!planetInstance) return;

            population.text = $"Population: {planetInstance.planetData.Population}";
            habitibility.text = $"Habitibility: {planetInstance.planetData.HabitabilityString}";

            bool inhabited = planetInstance.planetData.Population != 0;
            actionButtonNuke.gameObject.SetActive(false);

            bool hasCargo = false;
            bool hasSpace = false;

            if (planetInstance.IsPlayerOrbiting)
            {
                foreach (var res in resourceSources)
                {
                    if (PlayerShip.Instance.shipController.CargoController.SpaceFor(res.resource) > 0)
                    {
                        hasSpace = true;
                        break;
                    }
                }

                foreach (var res in resourceDemands)
                {
                    if (PlayerShip.Instance.shipController.CargoController.Has(res.resource))
                    {
                        hasCargo = true;
                        break;
                    }
                }
                visitButton.buttonText = "DEPART";
                visitButton.UpdateUI();
            }
            else if (PlayerInTransitToPlanet)
            {
                visitButton.buttonText = "Abort Visit";
                visitButton.UpdateUI();
            }
            else
            {
                visitButton.buttonText = "VISIT";
                visitButton.UpdateUI();
            }

            actionButtonMine.gameObject.SetActive(!inhabited && hasSpace);
            actionButtonTrade.gameObject.SetActive(inhabited && hasCargo);
            actionButtonSellAll.gameObject.SetActive(inhabited && hasCargo);

            UpdateMineableResources();
            UpdateDesiredResources();
        }

        private void UpdateMineableResources()
        {
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

        private void UpdateDesiredResources()
        {
            string desiredSources = "";
            if (resourceDemands.Length > 0)
            {
                foreach (var resourceSource in resourceDemands)
                {
                    if (desiredSources.Length > 0) desiredSources += "\n";
                    desiredSources +=
                        $"{resourceSource.resource.name}: {resourceSource.Price.ToString("F2")}";
                }
            }
            else
            {
                desiredSources = "None";
            }

            desiredResources.text = desiredSources;
        }

        public void Visit()
        {
            if (!planetInstance) return;

            if (planetInstance.IsPlayerOrbiting)
            {
                PlayerShip.Instance.shipController.MovementController.LeaveOrbit();
            }
            else if (PlayerInTransitToPlanet)
            {
                PlayerShip.MovementController.Stop();
            }
            else
            {
                PlayerShip.Instance.shipController.MovementController.Orbit(planetInstance);
            }
        }

        public bool PlayerInTransitToPlanet => planetInstance && !orbitingShip && !planetInstance.IsPlayerOrbiting &&
                                               PlayerShip.MovementController.OrbitPlanetTarget ==
                                               planetInstance;

        public void DeselectPlanet()
        {
            onDeselectPlanet?.Invoke(planetInstance);
        }

        public void Mine()
        {
            if (planetInstance && planetInstance.IsPlayerOrbiting && planetInstance.planetData.Population == 0)
            {
                for (int i = 0; i < resourceSources.Length; i++)
                {
                    var resource = resourceSources[i];
                    if (resource.ResourceAvailable)
                    {
                        PlayerShip.Instance.shipController.MiningController.Mine(resource);
                        break;
                    }
                }
            }
        }

        public void SellAll()
        {
            if (planetInstance && PlayerShip.Instance.shipController.TradeController && resourceDemands.Length > 0)
            {
                PlayerShip.Instance.shipController.TradeController.SellAll(planetInstance);
            }
        }

        public void OnShipEnteredOrbit(ShipController ship, PlanetInstance planet)
        {
            if (planet == planetInstance)
            {
                UpdateData();
            }
        }

        private void Update()
        {
            if (!planetInstance) return;

            if (!orbitingShip && planetInstance.IsPlayerOrbiting)
            {
                orbitingShip = true;
                UpdateData();
            }
            else if (orbitingShip && !planetInstance.IsPlayerOrbiting)
            {
                orbitingShip = false;
                UpdateData();
            }

            var d = Vector3.Distance(PlayerShip.Transform.position,
                planetInstance.transform.position).ToString("0");
            if (PlayerInTransitToPlanet)
            {
                if (visitButton.buttonText != "Abort Visit")
                {
                    visitButton.buttonText = "Abort Visit";
                    visitButton.UpdateUI();
                }


                distance.text = "Remaining Distance: " + d;
            }
            else
            {
                distance.text = "Distance: " + d;
            }
        }
    }
}
