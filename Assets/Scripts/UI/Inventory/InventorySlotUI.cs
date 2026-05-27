using GameDevTV.Core.UI.Dragging;
using NoName.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoName.UI
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<ItemSO>
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private GameObject _itemStack;
        [SerializeField] private TextMeshProUGUI _itemStackText;

        private int _inventoryIndex;

        public void Init(int index)
        {
            _inventoryIndex = index;
        }

        public void Draw(ItemSO item, int quantity = 1)
        {
            if (item == null)
            {
                SetIcon(null);

                _itemStack.SetActive(false);
            }
            else
            {
                SetIcon(item.ItemIcon);

                if (item.IsStackable)
                {
                    _itemStack.SetActive(true);
                    _itemStackText.text = quantity.ToString();
                }
                else
                {
                    _itemStack.SetActive(false);
                }
            }
        }

        private void SetIcon(Sprite sprite)
        {
            if (sprite == null)
            {
                _itemIcon.sprite = null;
                _itemIcon.enabled = false;
                return;
            }

            _itemIcon.sprite = sprite;
            _itemIcon.enabled = true;
        }

        public void AddItems(ItemSO item, int number)
        {
            InventoryManager.Instance.TryAddItemToSlot(_inventoryIndex, item, number);
        }

        public ItemSO GetItem()
        {
            return InventoryManager.Slots[_inventoryIndex].item;
        }

        public int GetNumber()
        {
            return InventoryManager.Slots[_inventoryIndex].quantity;
        }

        public int MaxAcceptable(ItemSO item)
        {
            if (InventoryManager.Slots[_inventoryIndex].item == null)
            {
                return int.MaxValue;
            }

            return 0;
        }

        public void RemoveItems(int number)
        {
            InventoryManager.Instance.RemoveItemFromSlot(_inventoryIndex, number);
        }
    }
}
