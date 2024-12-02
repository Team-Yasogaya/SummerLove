using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    [System.Serializable]
    public class DialogueAction
    {
        [System.Serializable]
        public enum ActionType : uint
        {
            RemoveItem,
            AddItem,
            Custom,
        }

        [SerializeField] private ActionType _actionType;

        [HideInInspector]
        [SerializeField] private string _itemId;
        [HideInInspector]
        [SerializeField] private int _quantity;
        [HideInInspector]
        [SerializeField] private int _undeadIndex;
        [HideInInspector]
        [SerializeField] private int _appraisalVariation;
        [HideInInspector]
        [SerializeField] private string _actionName;

        public ActionType Type { get { return _actionType; } }
        public string ItemId { get { return _itemId; } }
        public int Quantity { get { return _quantity; } }
        public string ActionName { get { return _actionName; } }
    }
}
