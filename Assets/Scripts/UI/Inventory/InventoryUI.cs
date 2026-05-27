using NoName.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoName.UI
{
    public class InventoryUI : BaseMenuUI
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private InventorySlotUI[] _inventorySlots;

        private void OnEnable()
        {
            InventoryManager.InventoryUpdated += Redraw;
        }

        private void OnDisable()
        {
            InventoryManager.InventoryUpdated -= Redraw;
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);

            Init();
        }

        private void Init()
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                _inventorySlots[i].Init(i);
            }
        }

        public override void Open()
        {
            base.Open();

            Redraw();
        }

        public override void Close()
        {
            base.Close();
        }

        private void Redraw()
        {
            for (int index = 0; index < InventoryManager.Slots.Length; index++)
            {
                var inventorySlot = _inventorySlots[index];
                inventorySlot.Draw(InventoryManager.Slots[index].item, InventoryManager.Slots[index].quantity);
            }
        }
    }
}
