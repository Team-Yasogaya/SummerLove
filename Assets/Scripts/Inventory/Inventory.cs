using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;

        [System.Serializable]
        public class InventorySlot
        {
            public Item item;
            public int quantity;
        }

        [SerializeField] private List<InventorySlot> _inventorySlots = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        public void StoreItem(Item item)
        {
            if (item.IsStackable)
            {
                InventorySlot inventorySlot = GetInventorySlotByItem(item);

                if (inventorySlot != null)
                {
                    inventorySlot.quantity += 1;
                }
                else
                {
                    inventorySlot = new InventorySlot();
                    inventorySlot.item = item;
                    inventorySlot.quantity = 1;
                    _inventorySlots.Add(inventorySlot);
                }
            }
            else
            {
                InventorySlot inventorySlot = new InventorySlot();
                inventorySlot.item = item;
                inventorySlot.quantity = 1;
                _inventorySlots.Add(inventorySlot);
            }
        }

        public void StoreItem(Item item, int quantity)
        {
            if (item.IsStackable)
            {
                InventorySlot inventorySlot = GetInventorySlotByItem(item);

                if (inventorySlot != null)
                {
                    inventorySlot.quantity += quantity;
                }
                else
                {
                    inventorySlot = new InventorySlot();
                    inventorySlot.item = item;
                    inventorySlot.quantity = quantity;
                    _inventorySlots.Add(inventorySlot);
                }
            }
            else
            {
                Debug.Log("You tried to store a non-stackable item with StoreItem(item, quantity). This should not happen.");
            }
        }

        private void ClearSlot(InventorySlot itemSlot)
        {
            _inventorySlots.Remove(itemSlot);
        }

        public InventorySlot GetInventorySlotByItem(Item item)
        {
            foreach (var slot in _inventorySlots)
            {
                if (slot.item == item)
                {
                    return slot;
                }
            }

            return null;
        }

        public IEnumerable<InventorySlot> GetAllItems()
        {
            return _inventorySlots;
        }
    }
}
