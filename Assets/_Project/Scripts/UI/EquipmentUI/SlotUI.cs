/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Data.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubTech.OpenPath.UI.EquipmentUI
{
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] private Image selectedOverlay;
        [SerializeField] private Image unselectedOverlay;
        [SerializeField] public Image slotIcon;
        [SerializeField] public GameObject countContainer;
        [SerializeField] public TextMeshProUGUI countText;

        public Action<SlotUI, AbstractShipEquipment> onSlotClicked;
        private AbstractShipEquipment equipment;
        public string type;

        public AbstractShipEquipment Equipment
        {
            get => equipment;
            set
            {
                if (equipment)
                {
                    slotIcon.sprite = equipment.icon;
                }
                else
                {
                    slotIcon.sprite = null;
                }

                equipment = value;
            }
        }

        private int count;

        public int Count
        {
            get => count;
            set
            {
                countContainer.SetActive(count > 1);
                countText.text = "" + value;
                count = value;
            }
        }

        public void OnSlotClicked()
        {
            onSlotClicked?.Invoke(this, equipment);
        }

        public bool Selected
        {
            get => selectedOverlay.gameObject.activeInHierarchy;
            set
            {
                selectedOverlay.gameObject.SetActive(value);
                unselectedOverlay.gameObject.SetActive(!value);
            }
        }
    }
}
