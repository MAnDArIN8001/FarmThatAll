using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Items;
using Storage.Setup;
using UnityEngine;

namespace Storage
{
    public class Storage : IStorage, IDisposable
    {
        public event Action<ItemType> OnStorageItemChanged;

        private readonly ItemsSetup _setup;
        
        private readonly Dictionary<ItemType, StorageItem> _storage = new();

        public Storage(ItemsSetup setup)
        {
            _setup = setup;
        }
        
        public List<StorageItem> GetItemsWithScope(ItemScope scope)
        {
            var scopeBinding = _setup.ItemBindings.FirstOrDefault(b => b.ItemScope == scope);
            
            if (scopeBinding == null)
                return new();

            var itemTypesSet = scopeBinding.Items
                .Select(i => i.ItemType)
                .ToHashSet();

            return _storage
                .Where(pair => itemTypesSet.Contains(pair.Key))
                .Select(pair => pair.Value)
                .ToList();
        }

        public int GetItemsCount(ItemType itemType)
        {
            if (_storage.TryGetValue(itemType, out var item))
            {
                return item.Count;
            }
            
            return 0;
        }

        public void IncreaseItem(ItemType itemType, int increaseCount)
        {
            if (increaseCount < 0)
            {
                Debug.LogError($"Increase value cant be less than zero");

                return;
            }
            
            if (!_storage.TryGetValue(itemType, out var item))
            {
                _storage.Add(itemType, new StorageItem(GetItemOfType(itemType)));
            }
            
            _storage[itemType].Count += increaseCount;
            
            OnStorageItemChanged?.Invoke(itemType);
        }

        public void DecreaseItem(ItemType itemType, int decreaseCount)
        {
            if (decreaseCount < 0)
            {
                Debug.LogError($"Decrease value can't be less than zero");

                return;
            }
            
            if (!_storage.TryGetValue(itemType, out var item))
            {
                Debug.LogError($"The storage {this} doesn't contain any item with type {itemType}");

                return;
            }

            if (decreaseCount > item.Count)
            {
                Debug.LogError($"Decrease value can't be more than current count");

                return;
            }
            
            item.Count -= decreaseCount;
            
            OnStorageItemChanged?.Invoke(itemType);
        }

        private Item GetItemOfType(ItemType itemType)
        {
            var item = _setup.ItemBindings
                .SelectMany(binding => binding.Items)
                .FirstOrDefault(item => item.ItemType == itemType);

            if (item is null)
            {
                Debug.LogError($"The Ssetup {_setup} doesn't contain any item with type {itemType}");

                return null;
            }
            
            return item;
        }
        
        public void Dispose()
        {
            _storage.Clear();
        }
    }
}