/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Data.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubTech.OpenPath.UI
{
    public class MainHud : MonoBehaviour
    {
        [SerializeField] private ShipController player;
        [SerializeField] private RectTransform resourceContainer;
        [SerializeField] private Resource resourcePrefab;
        [SerializeField] private ProductionResource[] trackedResources;
        [SerializeField] private Sprite currencyIcon;
        [SerializeField] private Sprite capacityIcon;
        [SerializeField, Tooltip("The frequency the UI should update in seconds.")]
        private float refreshFrequency = 0.25f;

        private Resource[] resourceeValues;
        private Resource currency;
        private Resource capacity;

        private void Start()
        {
            capacity = Instantiate(resourcePrefab, resourceContainer);
            capacity.icon.sprite = capacityIcon;

            resourceeValues = new Resource[trackedResources.Length];
            for (int i = 0; i < trackedResources.Length; i++)
            {
                resourceeValues[i] = Instantiate(resourcePrefab, resourceContainer);
                resourceeValues[i].icon.sprite = trackedResources[i].icon;
            }

            currency = Instantiate(resourcePrefab, resourceContainer);
            currency.icon.sprite = currencyIcon;

            player = GameManager.Instance.player;
            StartCoroutine(UpdateData());
        }

        public IEnumerator UpdateData()
        {
            yield return new WaitUntil(() => player != null);

            while (true)
            {
                float current = 0;
                float total = 0;
                if (player.CargoController)
                {
                    current += player.CargoController.AvailableCapacity;
                    total += player.CargoController.TotalCapacity;
                }
                if (player.MiningController)
                {
                    current += player.MiningController.capacity;
                    current -= player.MiningController.resource.quantity;
                    total += player.MiningController.capacity;
                }
                
                capacity.quantity.text =
                    (total - current).ToString("0") + "/" +
                    total.ToString("0");
                currency.quantity.text = player.Credits.ToString("F2");

                yield return null;

                for (int i = 0; i < trackedResources.Length; i++)
                {
                    float quantity = 0;
                    if (player.CargoController)
                    {
                        quantity += player.CargoController.Quantity(trackedResources[i]);
                    }
                    resourceeValues[i].quantity.text = quantity.ToString("0");
                    LayoutRebuilder.MarkLayoutForRebuild(resourceeValues[i].quantity.rectTransform);

                    yield return null;
                }

                resourceContainer.ForceUpdateRectTransforms();
                LayoutRebuilder.MarkLayoutForRebuild(resourceContainer);
                LayoutRebuilder.ForceRebuildLayoutImmediate(resourceContainer);

                yield return new WaitForSeconds(refreshFrequency);
            }
        }
    }
}
