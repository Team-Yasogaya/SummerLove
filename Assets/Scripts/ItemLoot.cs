using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName
{
    [CreateAssetMenu(fileName = "New ItemLoot", menuName = "Items/Item Loot")]
    public class ItemLoot : ScriptableObject, ISerializationCallbackReceiver
    {
        [Header("General")]
        [SerializeField] private Item _item;
        [SerializeField] private int _quantity;

        public Item Item { get { return _item; } }
        public int Quantity { get { return _quantity; } }

        public virtual void OnAfterDeserialize()
        {
            
        }

        public virtual void OnBeforeSerialize()
        {
            
        }
    }
}