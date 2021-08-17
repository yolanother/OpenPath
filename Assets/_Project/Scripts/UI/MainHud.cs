/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Controllers;
using TMPro;
using UnityEngine;

namespace DoubTech.OpenPath.UI
{
    public class MainHud : MonoBehaviour
    {
        [SerializeField] private ShipController playerShip;
        [SerializeField] private TextMeshProUGUI credits;

        [SerializeField] private RectTransform rectTransform;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void UpdateData()
        {
            credits.text = playerShip.Credits.ToString("F2");
            credits.SetLayoutDirty();
            rectTransform.ForceUpdateRectTransforms();
        }

        private void OnEnable()
        {
            UpdateData();
        }
    }
}
