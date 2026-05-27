using GameDevTV.Saving;
using Newtonsoft.Json.Linq;
using NoName.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace NoName.Item
{
    public class ItemPickup : Interactable, IJsonSaveable
    {
        [SerializeField] ItemLoot _itemLoot;
        [SerializeField] Collider _interactionCollider;
        [SerializeField] GameObject _pickupModel;

        private bool isCollected;

        public override void Interact()
        {
            if (InventoryManager.Instance.TryAddItemToFirstSlot(_itemLoot.ItemSO, _itemLoot.Quantity))
            {
                // UI PROMPT PICKED UP ITEM
                isCollected = true;
                
                UpdatePickUpStatus();
            }
            else
            {
                // UI PROMPT NOT EMPTY SLOT AVAILABLE
                Debug.Log("No empty slot available in inventory.");
            }
        }

        private void UpdatePickUpStatus() 
        {
            if (isCollected)
            {
                _interactionCollider.enabled = false;
                _pickupModel.SetActive(false);
            }
            else
            {
                _interactionCollider.enabled = true;
                _pickupModel.SetActive(true);
            }
        }

        public override void ShowPrompt()
        {
            
        }

        public override void HidePrompt()
        {
            
        }

        public JToken CaptureAsJToken()
        {
            JObject state = new JObject();
            IDictionary<string, JToken> stateDict = state;
            stateDict["isCollected"] = isCollected;
            return state;
        }

        public void RestoreFromJToken(JToken s)
        {
            JObject state = s.ToObject<JObject>();
            isCollected = state["isCollected"].ToObject<bool>();
            UpdatePickUpStatus();
        }
    }
}
