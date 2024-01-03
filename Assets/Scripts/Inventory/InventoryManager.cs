using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [System.Serializable]
        public struct InventorySlot
        {
            public ItemSO item;
            public int quantity;
        }

        [SerializeField] private InventorySlot[] _slots;

        public static InventorySlot[] Slots => Instance._slots;

        public static event Action InventoryUpdated;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _slots = new InventorySlot[12];
        }

        public bool TryAddItemToFirstSlot(ItemSO item, int quantity = 1)
        {
            int slotIndex = FindSlot(item);

            if (slotIndex < 0)
            {
                return false;
            }

            _slots[slotIndex].item = item;
            _slots[slotIndex].quantity += quantity;

            InventoryUpdated?.Invoke();

            return true;
        }

        public bool TryAddItemToSlot(int slotIndex, ItemSO item, int quantity = 1)
        {
            if (_slots[slotIndex].item != null && (
                _slots[slotIndex].item != item || _slots[slotIndex].item.IsStackable == false))
            {
                return false;
            }

            _slots[slotIndex].item = item;
            _slots[slotIndex].quantity += quantity;

            InventoryUpdated?.Invoke();

            return true;
        }

        public void RemoveItem(ItemSO item, int quantity = 1)
        {
            int slotIndex = FindSlot(item);

            if (slotIndex < 0)
            {
                Debug.Log("Item Slot not Found");
                return;
            }

            RemoveItemFromSlot(slotIndex, quantity);
        }

        public void RemoveItemFromSlot(int slotIndex, int quantity = 1)
        {
            _slots[slotIndex].quantity -= quantity;

            if (_slots[slotIndex].quantity <= 0)
            {
                _slots[slotIndex].quantity = 0;
                _slots[slotIndex].item = null;
            }

            InventoryUpdated?.Invoke();
        }

        private int FindSlot(ItemSO item)
        {
            if (item.IsStackable)
            {
                for (int i = 0; i < _slots.Length; i++)
                {
                    if (_slots[i].item == item)
                    {
                        return i;
                    }
                }
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item == null)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
