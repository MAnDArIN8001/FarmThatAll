using System.Linq;
using Building;
using Sounds;
using Storage;
using Storage.Setup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utiles;
using Utiles.EventSystem;
using Zenject;

namespace UI.Windows.Variants.TerminalWindows.Elements
{
    public class ShopCard : MonoBehaviour
    {
        private bool _isBuilding;
        private bool _isLevel;
        
        [Header("UI Elements")]
        [SerializeField] private Image _icon;

        [Space, SerializeField] private TMP_Text _name;

        [Space, SerializeField] private Button _byuButton;
        
        [Space, SerializeField] private GameObject _locker;

        [Space, SerializeField] private Transform _priceRoot;

        [Header("Configuration")] 
        [SerializeField] private PriceView _priceViewPrefab;
        
        [Space, SerializeField] private ItemsSetup _itemsSetup;
        
        private LevelManager _levelManager;

        [Inject] private SoundService _soundService;

        private Item _itemData;
        
        private IStorage _storage;
        
        private EventBus _eventBus;

        public void Initialize(Item itemData, IStorage storage, EventBus eventBus, bool isLocked, bool isBuilding = false)
        {
            _itemData = itemData;
            _storage = storage;
            _eventBus = eventBus;
            _isBuilding = isBuilding;
            
            _icon.sprite = itemData?.ItemSprite;
            _name.text = itemData.ItemName;
            
            _locker.SetActive(isLocked);

            foreach (var priceData in itemData.BuyPrice)
            {
                var priceView =  Instantiate(_priceViewPrefab, _priceRoot);
                
                var priceItemData = _itemsSetup.ItemBindings
                    .SelectMany(binding => binding.Items)
                    .FirstOrDefault(item => item.ItemType == priceData.ItemType);
                
                priceView.Initialize(priceData.Price.ToString(), priceItemData?.ItemSprite);
            }

            _byuButton.interactable = CheckPrice();
        }

        public void InitializeUpgrade(LevelManager levelManager)
        {
            _isLevel = true;
            _levelManager = levelManager;

            if (_levelManager.CurrentLevel != _itemData.TargetLevel)
            {
                _locker.SetActive(true);
            }

            _levelManager.OnLevelChanged += UpdateLocker;
        }

        private void OnEnable()
        {
            if (_storage is not null)
            {
                _byuButton.interactable = CheckPrice();
            }
            
            if (_byuButton is not null)
            {
                _byuButton.onClick.AddListener(HandleBuy);
            }
        }

        private void OnDisable()
        {
            if (_byuButton is not null)
            {
                _byuButton.onClick.RemoveListener(HandleBuy);
            }
        }

        private void UpdateLocker(int newLevel)
        {
            _locker.SetActive(newLevel != _itemData.TargetLevel);
        }

        private bool CheckPrice()
        {
            bool isEnoughResources = true;

            foreach (var priceData in _itemData.BuyPrice)
            {
                var countInStorage = _storage.GetItemsCount(priceData.ItemType);

                isEnoughResources = countInStorage >= priceData.Price;

                if (!isEnoughResources)
                {
                    break;
                }
            }
            
            return isEnoughResources;
        }

        private void HandleBuy()
        {
            foreach (var priceData in _itemData.BuyPrice)
            {
                _storage.DecreaseItem(priceData.ItemType, priceData.Price);
            }

            if (_isBuilding)
            {
                _eventBus.Publish(_itemData.BuildingData);
            }
            else if (_isLevel)
            {
                _levelManager.IncreaseLevel();
            }
            else
            {
                _storage.IncreaseItem(_itemData.ItemType, 1);
            }
            
            _byuButton.interactable = CheckPrice();
            _soundService.Play(SoundType.Music, "buy");
        }
    }
}