using System;
using System.Collections.Generic;
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

        private static Dictionary<string, ItemSO> itemLookupCache;

        public static ItemSO GetFromId(string itemId)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new();
                var itemList = Resources.LoadAll<ItemSO>("");

                foreach (var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.Id))
                    {
                        Debug.LogError("There is a duplicate Id in Items for objectId: " + item.Id);
                        continue;
                    }

                    itemLookupCache[item.Id] = item;
                }
            }

            if (itemId == null || !itemLookupCache.TryGetValue(itemId, out ItemSO value)) return null;

            return value;
        }

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
