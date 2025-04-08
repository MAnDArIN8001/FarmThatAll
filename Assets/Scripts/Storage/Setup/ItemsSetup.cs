using System;
using System.Collections.Generic;
using NUnit.Framework;
using Storage.Items;
using UnityEngine;

namespace Storage.Setup
{
    [CreateAssetMenu(menuName = "Setup/Items", fileName = "NewItemsSetup", order = 1)]
    public class ItemsSetup : ScriptableObject
    {
        [SerializeField] protected List<ItemBinding> _itemBindings;
        
        public IReadOnlyList<ItemBinding> ItemBindings => _itemBindings;
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

        public Item(string itemName, ItemType itemType)
        {
            ItemName = itemName;
            ItemType = itemType;
        }
        
        public Item() {}
    }
}