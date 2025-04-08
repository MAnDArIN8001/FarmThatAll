using System;
using System.Collections.Generic;
using Storage.Items;
using Storage.Setup;
using UnityEngine;

namespace Storage
{
    public interface IStorage
    {
        public event Action<ItemType> OnStorageItemChanged;
        
        public List<StorageItem> GetItemsWithScope(ItemScope scope);
        
        public int GetItemsCount(ItemType itemType);

        public void IncreaseItem(ItemType itemType, int count);
        public void DecreaseItem(ItemType itemType, int count);
    }
    
    [Serializable]
    public class StorageItem
    {
        private int _count = 0;

        public int Count
        {
            get => _count;
            set
            {
                if (value < 0)
                {
                    Debug.LogError("Value cannot be negative");

                    return;
                }
                
                _count = value;  
            } 
        }

        public Item Item { get; private set; }

        public StorageItem(Item item)
        {
            Item = item;
            _count = 0;
        }

        public StorageItem(Item item, int count)
        {
            Item = item;
            _count = count;
        }
    }
}