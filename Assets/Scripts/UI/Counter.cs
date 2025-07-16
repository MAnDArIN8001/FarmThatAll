using System;
using System.Linq;
using Storage.Items;
using Storage.Setup;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class Counter : MonoBehaviour
    {
        [SerializeField] private ItemType _countingResource;

        [Space, SerializeField] private ItemsSetup _itemsSetup;
        
        [Header("UI Elements")] 
        [SerializeField] private Image _icon;

        [Space, SerializeField] private TMP_Text _counterText;
        
        [Inject] private Storage.Storage _storage;

        private void Awake()
        {
            _storage.IncreaseItem(ItemType.Money, 100);
            
            var itemData = _itemsSetup.ItemBindings
                .SelectMany(binding => binding.Items)
                .FirstOrDefault(item => item.ItemType == _countingResource);
            
            _icon.sprite = itemData.ItemSprite;
            
            UpdateValue(_countingResource);
        }

        private void OnEnable()
        {
            if (_storage is not null)
            {
                _storage.OnStorageItemChanged += UpdateValue;
            }
        }

        private void OnDisable()
        {
            if (_storage is not null)
            {
                _storage.OnStorageItemChanged -= UpdateValue;
            }
        }

        private void UpdateValue(ItemType changedItem)
        {
            if (changedItem != _countingResource)
            {
                return;
            }
            
            var newCount = _storage.GetItemsCount(_countingResource);
            
            _counterText.text = newCount.ToString();
        }
    }
}