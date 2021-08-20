/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Events;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UI.PreviewCamera;
using DoubTech.OpenPath.UniverseScope;
using DoubTech.OpenPath.UniverseScope.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubTech.OpenPath.UI
{
    public class ShipInformationUI : MonoBehaviour
    {
        [SerializeField] private Image shipIcon;
        [SerializeField] private Image factionIcon;
        [SerializeField] private Image hull;
        [SerializeField] private Image shield;
        [SerializeField] private TextMeshProUGUI shipName;
        [SerializeField] private ShipGameEvent onSelectShip;

        private ShipController shipController;
        public ShipController Ship
        {
            get => shipController;
            set
            {
                shipController = value;
                UpdateData();
            }
        }

        public void UpdateData()
        {
            if (shipController == null) return;

            shipName.text = shipController.name;
            if (shipController.faction)
            {
                factionIcon.gameObject.SetActive(true);
                factionIcon.sprite = shipController.faction.factionEmblem;
                factionIcon.color = shipController.faction.factionColor;
            }
            else
            {
                factionIcon.gameObject.SetActive(false);
            }

            if (shield != null)
            {
                shield.fillAmount = shipController.DamageController.PercentHitPoints;
            }

            if (!shipIcon.sprite)
            {
                PreviewCameraController.Instance.TakeScreenshot(shipController, (s) => shipIcon.sprite = s);
            }
        }

        public void SelectShip()
        {
            onSelectShip.Invoke(shipController);
        }
    }
}
