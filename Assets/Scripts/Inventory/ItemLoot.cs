using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName.Inventory
{
    [System.Serializable]
    public class ItemLoot
    {
        [SerializeField] private ItemSO _itemSO;
        [SerializeField] private int _quantity;

        public ItemSO ItemSO => _itemSO;
        public int Quantity => _quantity;
    }
}
