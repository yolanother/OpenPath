/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections.Generic;
using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.SolarSystemScope;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace DoubTech.OpenPath.UI
{
    public class SystemInfoList : MonoBehaviour
    {
        [SerializeField] private PlanetResourceInformation resourceInformationPrefab;
        [SerializeField] private RectTransform container;

        private SolarSystemInstance solarSystemInstance;
        private bool initialized = false;

        private Dictionary<PlanetInstance, PlanetResourceInformation> resourceInfo = new Dictionary<PlanetInstance, PlanetResourceInformation>();

        private void Start()
        {
            solarSystemInstance = FindObjectOfType<SolarSystemInstance>();
        }

        private void Update()
        {
            // TODO: We should do this better. This is a quick hack to wait for planets to be initialized.
            if (!initialized && null != solarSystemInstance.Planets && solarSystemInstance.Planets.Length > 0)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            initialized = true;
            foreach (var planet in solarSystemInstance.Planets)
            {
                var info = Instantiate(resourceInformationPrefab, container);
                info.PlanetInstance = planet;
                resourceInfo[planet] = info;
            }
        }

        public void UpdatePlanet(PlanetInstance planet)
        {
            if (resourceInfo.TryGetValue(planet, out var info))
            {
                info.UpdateData();
            }
        }
    }
}
