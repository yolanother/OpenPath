/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Data.Equipment;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UI.PreviewCamera;
using Lean.Common.Editor;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubTech.OpenPath.UI.EquipmentUI
{
    public class EquipmentView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI selectedEquipmentName;
        [SerializeField] private Image currentItemTypeIcon;
        [SerializeField] private HorizontalSelector itemTypeSelector;

        [SerializeField] private ButtonManagerBasic buy;
        [SerializeField] private ButtonManagerBasic sell;

        [SerializeField] private RectTransform purchasableItemsGrid;
        [SerializeField] private RectTransform equippedItemsGrid;

        [SerializeField] private SlotUI slotPrefab;

        private TradeController currentTradePartner;

        private void OnEnable()
        {
            SetEquipmentType(itemTypeSelector.index);
        }

        public void FocusCamera()
        {
            PreviewCameraController.Instance.FocusCameraOn(PlayerShip.Instance.shipController);
        }

        public void SetEquipmentType(int type)
        {
            for (int i = equippedItemsGrid.childCount - 1; i >= 0; i++)
            {
                Destroy(equippedItemsGrid.GetChild(i).gameObject);
            }
            switch (type)
            {
                case 0:
                    ShowCargoBaySlots();
                    break;
                case 1:
                    ShowWeaponSlots();
                    break;
                case 2:
                    ShowShieldSlots();
                    break;
            }
        }

        private void ShowShieldSlots()
        {

        }

        private void ShowCargoBaySlots()
        {

        }

        private void ShowWeaponSlots()
        {

            var weaponSlots = PlayerShip.Instance.shipController.GetComponents<ShipWeaponController>();
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                var weaponSlot = weaponSlots[i];
                var slot = Instantiate(slotPrefab, equippedItemsGrid);
                slot.type = "Weapon Slot";
                slot.Count = 0;
                if (weaponSlot.weapon)
                {
                    slot.Equipment = weaponSlot.weapon;
                }

                slot.onSlotClicked += OnEquippedWeaponSlotCliked;
            }

            /*var tradeSlots = currentTradePartner.GetComponents<Offer>();
            for (int i = 0; i < tradeSlots.Length; i++)
            {
                var weaponSlot = tradeSlots[i];
                var slot = Instantiate(slotPrefab, equippedItemsGrid);
                slot.type = "Weapon";
                if (weaponSlot.weapon)
                {
                    slot.equipment = weaponSlot.weapon;
                }

                slot.onSlotClicked += OnPurchasableWeaponClicked;
            }*/
        }

        private void OnPurchasableWeaponClicked(SlotUI selectedSlot,
            AbstractShipEquipment selectedEquipment)
        {
            selectedEquipmentName.text = selectedEquipment
                ? selectedEquipment.name
                : $"Empty {selectedSlot.type} Slot";

        }

        private void OnEquippedWeaponSlotCliked(SlotUI selectedSlot, AbstractShipEquipment selectedEquipment)
        {
            selectedEquipmentName.text = selectedEquipment ? selectedEquipment.name : $"Empty {selectedSlot.type} Slot";
        }

        public void StartTrading(PlanetInstance instance)
        {
            currentTradePartner = instance.GetComponent<TradeController>();
        }

        public void Buy()
        {

        }

        public void Sell()
        {

        }
    }
}
