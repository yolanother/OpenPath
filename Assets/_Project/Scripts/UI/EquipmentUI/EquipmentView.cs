/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.Data.Equipment;
using DoubTech.OpenPath.Eqipment;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.OpenPath.UI.PreviewCamera;
using DoubTech.OpenPath.UniverseScope.Equipment;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubTech.OpenPath.UI.EquipmentUI
{
    public class EquipmentView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI selectedPurchasableEquipmentName;
        [SerializeField] private TextMeshProUGUI selectedInventoryItemName;
        [SerializeField] private Image currentItemTypeIcon;
        [SerializeField] private HorizontalSelector itemTypeSelector;

        [SerializeField] private ButtonManagerBasic buy;
        [SerializeField] private ButtonManagerBasic sell;

        [SerializeField] private RectTransform purchasableItemsGrid;
        [SerializeField] private RectTransform equippedItemsGrid;

        [SerializeField] private SlotUI slotPrefab;

        [SerializeField] private Sprite[] categoryIcons;

        private TradeController currentTradePartner;
        private PlanetInstance tradePlanet;
        private SlotUI purchaseItemSlot;
        private SlotUI inventoryItemSlot;
        private EquipmentTrade activePurchasable;
        private EquipmentTrade activeSellable;

        private void OnEnable()
        {
            SetEquipmentType(itemTypeSelector.index);
        }

        public void FocusCamera()
        {
            PreviewCameraController.Instance.FocusCameraOn(GameManager.Instance.player);
        }

        public void SetEquipmentType(int type)
        {
            for (int i = equippedItemsGrid.childCount - 1; i >= 0; i--)
            {
                Destroy(equippedItemsGrid.GetChild(i).gameObject);
            }

            for (int i = purchasableItemsGrid.childCount - 1; i >= 0; i--)
            {
                Destroy(purchasableItemsGrid.GetChild(i).gameObject);
            }
            switch (type)
            {
                case 0:
                    ShowSlots<CargoPod>("Cargo Bay");
                    break;
                case 1:
                    ShowSlots<AbstractShipWeapon>("Weapon");
                    ShowWeaponSlots();
                    break;
                case 2:
                    ShowSlots<ShipShieldEquipment>("Cargo Bay");
                    break;
            }

            currentItemTypeIcon.sprite = categoryIcons[type];
        }

        private void ShowSlots<TYPE>(string equipmentName) where TYPE : AbstractShipEquipment
        {
            List<EquipmentTrade> tradeSlots = tradePlanet.GetForSale<TYPE>();
            for (int i = 0; i < tradeSlots.Count; i++)
            {
                EquipmentTrade offer = tradeSlots[i];
                SlotUI slot = Instantiate(slotPrefab, purchasableItemsGrid);
                slot.type = equipmentName;
                slot.Equipment = offer.equipment;
                slot.EquipmentTrade = offer;
                slot.onSlotClicked += OnPurchasableWeaponClicked;
            }
        }

        private void ShowWeaponSlots()
        {
            var weaponSlots = GameManager.Instance.player.AllWeaponControllers;
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                ShipWeaponController weaponSlot = weaponSlots[i];
                SlotUI slot = Instantiate(slotPrefab, equippedItemsGrid);
                slot.type = "Weapon Slot";
                slot.Count = 0;
                slot.Equipment = weaponSlot.EquippedWeapon;
                slot.onSlotClicked += OnEquippedWeaponSlotCliked;
            }
        }

        private void OnPurchasableWeaponClicked(SlotUI selectedSlot,
            AbstractShipEquipment selectedEquipment)
        {
            selectedPurchasableEquipmentName.text = selectedEquipment
                ? selectedEquipment.name
                : $"Empty {selectedSlot.type}";

            if (selectedSlot.EquipmentTrade)
            {
                selectedPurchasableEquipmentName.text +=
                    "\nAsking $" + selectedSlot.EquipmentTrade.AskingPrice.ToString("F");
            }

            buy.gameObject.SetActive(true);
            activePurchasable = selectedSlot.EquipmentTrade;

            if (purchaseItemSlot) purchaseItemSlot.Selected = false;
            purchaseItemSlot = selectedSlot;
            selectedSlot.Selected = true;
        }

        private void OnEquippedWeaponSlotCliked(SlotUI selectedSlot, AbstractShipEquipment selectedEquipment)
        {
            selectedInventoryItemName.text = selectedEquipment ? selectedEquipment.name : $"Empty {selectedSlot.type} Slot";
            if (inventoryItemSlot) inventoryItemSlot.Selected = false;
            inventoryItemSlot = selectedSlot;
            selectedSlot.Selected = true;


            bool sellable = selectedSlot.Equipment;
            if (sellable)
            {
                var sellableItems =
                    tradePlanet.GetSellable<AbstractShipEquipment>(selectedSlot.Equipment);
                if (sellableItems.Count > 0)
                {
                    activeSellable = sellableItems[0];
                    sellable = true;
                    selectedInventoryItemName.text += "\n[$" + activeSellable.OfferPrice.ToString("F") + "]";
                }
            }

            sell.gameObject.SetActive(sellable);
            activePurchasable = null;
        }

        public void StartTrading(PlanetInstance instance)
        {
            tradePlanet = instance;
        }

        public void Buy()
        {
            if (activePurchasable)
            {
                activePurchasable.Buy();
            }
        }

        public void Sell()
        {
            if (activeSellable)
            {

            }
        }
    }
}
