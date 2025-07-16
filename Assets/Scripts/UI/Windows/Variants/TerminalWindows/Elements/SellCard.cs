using System;
using Sounds;
using Storage;
using Storage.Items;
using Storage.Setup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.Variants.TerminalWindows.Elements
{
    public class SellCard : MonoBehaviour
    {
        public event Action<SellCard> OnTargetResourceEnded;

        private int _currentTotalCount;

        [Header("UI Elements")] 
        [SerializeField] private Image _icon;
        [Space, SerializeField] private TMP_Text _availableCounter;
        [SerializeField] private TMP_Text _totalCounter;
        [SerializeField] private TMP_Text _totalPrice;

        [Space, SerializeField] private Slider _totalSlider;
        
        [Space, SerializeField] private Button _sellButton;

        [Inject] private SoundService _soundService;

        private Item _itemData;
        
        private IStorage _storage;

        public void Initialize(IStorage storage, Item itemData)
        {
            _storage = storage;
            _itemData = itemData;
            
            _totalSlider.wholeNumbers = true;
            _icon.sprite = itemData.ItemSprite;

            _storage.OnStorageItemChanged += HandleStorageUpdate;
            
            var resourcesCount = _storage.GetItemsCount(_itemData.ItemType);

            _totalSlider.maxValue = resourcesCount;
            _availableCounter.text = resourcesCount.ToString();
        }

        private void OnEnable()
        {
            if (_totalSlider is not null)
            {
                _totalSlider.onValueChanged.AddListener(HandleValueChanged);
            }

            if (_sellButton is not null)
            {
                _sellButton.onClick.AddListener(HandlePurchased);
            }
        }

        private void OnDisable()
        {
            if (_totalSlider is not null)
            {
                _totalSlider.onValueChanged.RemoveListener(HandleValueChanged);
            }

            if (_sellButton is not null)
            {
                _sellButton.onClick.RemoveListener(HandlePurchased);
            }
        }

        private void OnDestroy()
        {
            if (_storage is not null)
            {
                _storage.OnStorageItemChanged -= HandleStorageUpdate;
            }
        }

        private void HandleValueChanged(float value)
        {
            var parsed = (int)value;
            
            _currentTotalCount = parsed;
            _totalPrice.text = $"{_currentTotalCount * _itemData.SellPrice.Price}$";
            Debug.Log(_itemData.SellPrice.Price);
            _totalCounter.text = $"{_currentTotalCount}";
        }

        private void HandlePurchased()
        {
            var count = (int)_totalSlider.value;
            
            _storage.DecreaseItem(_itemData.ItemType, count);
            _storage.IncreaseItem(_itemData.SellPrice.ItemType, _itemData.SellPrice.Price * count);

            _soundService.Play(SoundType.Music, "sell");
        }

        private void HandleStorageUpdate(ItemType itemType)
        {
            if (itemType != _itemData.ItemType)
            {
                return;
            }
            
            var newCount = _storage.GetItemsCount(itemType);

            if (newCount == 0)
            {
                OnTargetResourceEnded?.Invoke(this);
                
                Destroy(gameObject);

                return;
            }
            
            _availableCounter.text = newCount.ToString();

            if (_totalSlider.value > newCount)
            {
                _totalSlider.value = newCount;
            }
            
            _totalSlider.maxValue = newCount;
        }

        public void Remove()
        {   
            Destroy(gameObject);
        }
    }
}