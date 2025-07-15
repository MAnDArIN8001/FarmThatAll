using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Items;
using UnityEngine;
using Building;

namespace Storage.Setup
{
    [CreateAssetMenu(menuName = "Setup/Items", fileName = "NewItemsSetup", order = 1)]
    public class ItemsSetup : ScriptableObject
    {
        [SerializeField] protected List<ItemBinding> _itemBindings;
        
        public IReadOnlyList<ItemBinding> ItemBindings => _itemBindings;

        public ItemScope GetScopeOfItem(ItemType itemType)
        {
            foreach (var itemBinding in _itemBindings)
            {
                var containsItem = itemBinding.Items.Any(item => item.ItemType == itemType);

                if (containsItem)
                {
                    return itemBinding.ItemScope;
                }
            }

            return ItemScope.Default;
        }
    }

    [Serializable]
    public class ItemBinding
    {
        [field: SerializeField, Space] public ItemScope ItemScope { get; private set; }

        [SerializeField] private List<Item> _items;
        
        public IReadOnlyList<Item> Items => _items;

        public ItemBinding(ItemScope itemScope, List<Item> items)
        {
            ItemScope = itemScope;
            _items = items;
        }

        public ItemBinding()
        {
        }
    }

    [Serializable]
    public class Item
    {
        [field: SerializeField] public string ItemName { get; private set; }
        
        [field: SerializeField] public ItemType ItemType { get; private set; }
        
        [field: SerializeField, Space] public Sprite ItemSprite { get; private set; }
        
        [Header("Selling Data")]
        [field: SerializeField, Space] public PriceData  SellPrice { get; private set; }
        
        [Header("Buying Data")]
        [field: SerializeField] public List<PriceData> BuyPrice { get; private set; }
        
        [Header("Building Data")]
        [field: SerializeField] public BuildingData BuildingData { get; private set; }
        
        [Header("Level Data")]
        [field: SerializeField] public int TargetLevel { get; private set; }

        public Item(string itemName, ItemType itemType)
        {
            ItemName = itemName;
            ItemType = itemType;
        }
        
        public Item() {}
    }

    [Serializable]
    public class PriceData
    {
        [field: SerializeField] public int Price { get; private set; }
        
        [field: SerializeField, Space] public ItemType ItemType { get; private set; }
    }
}