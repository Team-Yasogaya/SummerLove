using NoName.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NoName
{
    public class DialogueTrigger : MonoBehaviour
    {
        public static void Trigger(DialogueAction action, DialogueTrigger customTrigger)
        {
            switch (action.Type)
            {
                case DialogueAction.ActionType.RemoveItem:
                    InventoryManager.Instance.RemoveItem(ItemSO.GetFromId(action.ItemId), action.Quantity);
                    break;
                case DialogueAction.ActionType.AddItem:
                    InventoryManager.Instance.TryAddItemToFirstSlot(ItemSO.GetFromId(action.ItemId), action.Quantity);
                    break;
                case DialogueAction.ActionType.Custom:
                    if (customTrigger == null)
                    {
                        Debug.LogWarning("A custom action trigger has been defined but the DialogueTrigger is NULL");
                        return;
                    }

                    customTrigger.Trigger(action.ActionName);
                    break;
            }
        }

        [System.Serializable]
        internal class ActionTrigger
        {
            public string ActionName;
            public UnityEvent OnTrigger; 
        }

        [SerializeField] private ActionTrigger[] _actionTriggers;

        private void Trigger(string actionToTrigger)
        {
            foreach (ActionTrigger actionTrigger in _actionTriggers)
            {
                if (actionToTrigger.Equals(actionTrigger.ActionName) == false) continue;
                
                actionTrigger.OnTrigger.Invoke();
            }
        }

        
    }
}
