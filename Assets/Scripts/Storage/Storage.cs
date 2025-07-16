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
        
        public static Storage Instance { get; private set; }

        public Storage(ItemsSetup setup)
        {
            _setup = setup;
            
            Instance = this;
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

        public StorageItem GetItemOfType(ItemType itemType)
        {
            if (!_storage.TryGetValue(itemType, out var item))
            {
                Debug.LogError($"The storage {this} doesnt contains any item of type {itemType}");

                return null;
            }

            return item;
        }

        public int GetItemsCount(ItemType itemType) => _storage.TryGetValue(itemType, out var item) ? item.Count : 0;

        public void IncreaseItem(ItemType itemType, int increaseCount)
        {
            if (increaseCount < 0)
            {
                Debug.LogError($"Increase value cant be less than zero");

                return;
            }
            
            if (!_storage.TryGetValue(itemType, out var item))
            {
                _storage.Add(itemType, new StorageItem(GetItemOfTypeFromSetup(itemType), GetScopeOfItem(itemType)));
            }
            
            _storage[itemType].Count += increaseCount;
            
            OnStorageItemChanged?.Invoke(itemType);
        }

        public void DecreaseItem(ItemType itemType, int decreaseCount)
        {
            if (decreaseCount < 0)
            {
                Debug.LogWarning($"Decrease value can't be less than zero");

                return;
            }
            
            if (!_storage.TryGetValue(itemType, out var item))
            {
                Debug.LogWarning($"The storage doesn't contain any item with type {itemType}");

                return;
            }

            if (decreaseCount > item.Count)
            {
                Debug.LogWarning($"Decrease value can't be more than current count");

                return;
            }
            
            Debug.Log($"{decreaseCount} {item.Count}");
            
            item.Count -= decreaseCount;

            if (item.Count == 0)
            {
                _storage.Remove(itemType);
            }
            
            OnStorageItemChanged?.Invoke(itemType);
        }

        private Item GetItemOfTypeFromSetup(ItemType itemType)
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

        private ItemScope GetScopeOfItem(ItemType itemType)
        {
            var itemBinding = _setup.ItemBindings.FirstOrDefault(itemBinding =>
                itemBinding.Items.Any(item => item.ItemType == itemType));

            if (itemBinding is null)
            {
                Debug.LogError($"Setup doesn't contains any item for with type {itemType}");

                return ItemScope.Default;
            }

            return itemBinding.ItemScope;
        }
        
        public void Dispose()
        {
            _storage.Clear();
        }
    }
}