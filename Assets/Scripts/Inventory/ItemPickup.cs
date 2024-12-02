using NoName.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName.Item
{
    public class ItemPickup : Interactable
    {
        [SerializeField] private ItemLoot _itemLoot;

        public override void Interact()
        {
            if (InventoryManager.Instance.TryAddItemToFirstSlot(_itemLoot.ItemSO, _itemLoot.Quantity))
            {
                // UI PROMPT PICKED UP ITEM
                Destroy(gameObject);
            }
            else
            {
                // UI PROMPT NOT EMPTY SLOT AVAILABLE
            }
        }

        public override void ShowPrompt()
        {
            
        }

        public override void HidePrompt()
        {
            
        }
    }
}
