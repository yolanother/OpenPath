/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Data.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubTech.OpenPath.UI
{
    public class MainHud : MonoBehaviour
    {
        [SerializeField] private ShipController playerShip;
        [SerializeField] private RectTransform resourceContainer;
        [SerializeField] private Resource resourcePrefab;
        [SerializeField] private ProductionResource[] trackedResources;
        [SerializeField] private Sprite currencyIcon;
        private Resource[] resourceeValues;
        private Resource currency;

        private void Start()
        {
            resourceeValues = new Resource[trackedResources.Length];
            for (int i = 0; i < trackedResources.Length; i++)
            {
                resourceeValues[i] = Instantiate(resourcePrefab, resourceContainer);
                resourceeValues[i].icon.sprite = trackedResources[i].icon;
            }

            currency = Instantiate(resourcePrefab, resourceContainer);
            currency.icon.sprite = currencyIcon;
        }

        public void UpdateData()
        {
            currency.quantity.text = playerShip.Credits.ToString("F2");

            for (int i = 0; i < trackedResources.Length; i++)
            {
                resourceeValues[i].quantity.text = playerShip.CargoController
                    .Quantity(trackedResources[i]).ToString("F");
                LayoutRebuilder.MarkLayoutForRebuild(resourceeValues[i].quantity.rectTransform);
            }


            resourceContainer.ForceUpdateRectTransforms();
            LayoutRebuilder.MarkLayoutForRebuild(resourceContainer);
            LayoutRebuilder.ForceRebuildLayoutImmediate(resourceContainer);
        }

        private void OnEnable()
        {
            UpdateData();
        }
    }
}
