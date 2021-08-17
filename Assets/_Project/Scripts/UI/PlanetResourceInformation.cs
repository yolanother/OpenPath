/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UniverseScope;
using DoubTech.OpenPath.UniverseScope.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubTech.OpenPath.UI
{
    public class PlanetResourceInformation : MonoBehaviour
    {
        [SerializeField] private Image planetIcon;
        [SerializeField] private TextMeshProUGUI planetName;
        [SerializeField] private TextMeshProUGUI resourceLabel;
        [SerializeField] private RectTransform resourceIconContainer;
        [SerializeField] private TextMeshProUGUI population;
        [SerializeField] private GameObject noResources;
        [SerializeField] private Image resourceIconPrefab;
        [SerializeField] private PlanetSelectionGameEvent onSelectPlanet;

        private PlanetInstance planetInstance;
        public PlanetInstance PlanetInstance
        {
            get => planetInstance;
            set
            {
                planetInstance = value;
                UpdateData();
            }
        }

        private void UpdateMineableResources()
        {
            var resourceSources = planetInstance.GetComponents<ResourceSource>();

            string mineableSources = "";
            if (resourceSources.Length > 0)
            {
                noResources.SetActive(false);
                foreach (var resourceSource in resourceSources)
                {
                    var icon = Instantiate(resourceIconPrefab, resourceIconContainer);
                    icon.sprite = resourceSource.resource.icon;
                }
            }
            else
            {
                noResources.SetActive(true);
            }
        }

        private void UpdateDesiredResources()
        {
            var resourceDemands = planetInstance.GetComponents<ResourceDemand>();
            string desiredSources = "";
            if (resourceDemands.Length > 0)
            {
                noResources.SetActive(false);
                foreach (var resourceSource in resourceDemands)
                {
                    var icon = Instantiate(resourceIconPrefab, resourceIconContainer);
                    icon.sprite = resourceSource.resource.icon;
                }
            }
            else
            {
                noResources.SetActive(true);
            }
        }

        public void UpdateData()
        {
            planetName.text = planetInstance.planetData.DisplayName;
            if (planetInstance.planetData.Population > 0)
            {
                resourceLabel.text = "Needed Resources";
                UpdateDesiredResources();
            }
            else
            {
                resourceLabel.text = "Mineable Resources";
                UpdateMineableResources();
            }

            population.text = "Population: " + planetInstance.planetData.Population;
        }

        public void SelectPlanet()
        {
            onSelectPlanet.Invoke(planetInstance);
        }
    }
}
