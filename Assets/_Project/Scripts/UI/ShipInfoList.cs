/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.SolarSystemScope;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace DoubTech.OpenPath.UI
{
    public class ShipInfoList : MonoBehaviour
    {
        [SerializeField] private ShipInformationUI shipInfoPrefab;
        [SerializeField] private RectTransform container;

        private SolarSystemInstance solarSystemInstance;
        private bool initialized = false;

        private Dictionary<ShipController, ShipInformationUI> ships = new Dictionary<ShipController,
            ShipInformationUI>();

        private void Start()
        {
            foreach (var ship in FindObjectsOfType<ShipController>())
            {
                UpdateShip(ship);
            }
        }

        public void UpdateShip(ShipController ship)
        {
            if (!ships.TryGetValue(ship, out var shipInfo))
            {
                shipInfo = Instantiate(shipInfoPrefab, container);
                ships[ship] = shipInfo;
                shipInfo.Ship = ship;
            }
            shipInfo.UpdateData();
        }
    }
}
