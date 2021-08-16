/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.SolarSystemScope;
using TMPro;
using UnityEngine;

namespace DoubTech.OpenPath.UI
{
    public class PlanetHud : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;

        [SerializeField] private PlanetSelectionGameEvent onVisitPlanet;
        [SerializeField] private PlanetSelectionGameEvent onLeavePlanetEvent;
        [SerializeField] private PlanetSelectionGameEvent onDeselectPlanet;

        private PlanetInstance planetInstance;

        public PlanetInstance PlanetInstance
        {
            get => planetInstance;
            set
            {
                planetInstance = value;
                title.text = value.planetData.DisplayName;
            }
        }

        public void Visit()
        {
            onVisitPlanet.Invoke(planetInstance);
        }

        public void Leave()
        {
            onLeavePlanetEvent.Invoke(planetInstance);
        }

        public void DeselectPlanet()
        {
            onDeselectPlanet?.Invoke(planetInstance);
        }
    }
}
