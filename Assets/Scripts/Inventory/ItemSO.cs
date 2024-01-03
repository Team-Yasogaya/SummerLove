using System;
using UnityEngine;

namespace NoName.Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
    public class ItemSO : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [TextArea]
        [SerializeField] private string _description;
        [SerializeField] private Sprite _itemIcon;
        [SerializeField] private bool _isStackable;
        [SerializeField] private bool _isCombinable;
        [SerializeField] private ItemSO _complementaryItem;
        [SerializeField] private ItemSO _resultingItem;
        [SerializeField] private int _resultingItemQuantity;

        public string Id => _id;
        public string Name => _name;
        public string Description => _description;
        public Sprite ItemIcon => _itemIcon;
        public bool IsStackable => _isStackable;
        public bool IsCombinable => _isCombinable;
        public ItemSO ComplementaryItem => _complementaryItem;
        public ItemSO ResultingItem => _resultingItem;
        public int ResultingItemQuantity => _resultingItemQuantity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
            }

            if (_isCombinable)
            {
                _isStackable = false;
            }

            if (_isStackable)
            {
                _complementaryItem = null;
                _resultingItem = null;
                _resultingItemQuantity = 0;
            }
        }
#endif
    }
}
