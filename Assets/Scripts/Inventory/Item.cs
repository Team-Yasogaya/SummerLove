using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
    public class Item : ScriptableObject, ISerializationCallbackReceiver
    {
        [Header("Info")]
        [SerializeField] private string _itemId;
        [SerializeField] private string _name;
        [TextArea]
        [SerializeField] private string _description;
        [SerializeField] private Sprite _itemIcon;

        [Header("General Settings")]
        [SerializeField] protected bool _isStackable;

        public string ItemId { get { return _itemId; } }
        public string Name { get { return _name; } }
        public string Description { get { return _description; } }
        public Sprite ItemIcon { get { return _itemIcon; } }
        public bool IsStackable { get { return _isStackable; } }

        // Utilities
        private static Dictionary<string, Item> itemLookupCache;

        public static Item GetFromId(string itemId)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new();
                var itemList = Resources.LoadAll<Item>("");

                foreach (var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.ItemId))
                    {
                        Debug.LogError("There is a duplicate Id in Items for objectId: " + item.ItemId);
                        continue;
                    }

                    itemLookupCache[item.ItemId] = item;
                }
            }

            if (itemId == null || !itemLookupCache.TryGetValue(itemId, out Item value)) return null;

            return value;
        }

        public virtual void OnAfterDeserialize()
        {
            
        }

        public virtual void OnBeforeSerialize()
        {
            if (string.IsNullOrEmpty(_itemId))
            {
                _itemId = System.Guid.NewGuid().ToString();
            }
        }
    }
}
