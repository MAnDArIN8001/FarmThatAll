using System.Collections.Generic;
using System.Linq;
using Ship;
using Storage;
using Storage.Items;
using Storage.Setup;
using UI.Filter;
using UnityEngine;
using UI.Windows.Variants.TerminalWindows.Elements;
using Utiles;
using Utiles.EventSystem;

namespace UI.Windows.Variants.TerminalWindows
{
    public class ShopWindow : AbstractWindow
    {
        [Header("UI")] 
        [SerializeField] private Transform _cardsRoot;
        
        [Space, SerializeField] private ShopCard _shopCardPrefab;

        [Space, SerializeField] private ScopeFilterManager _filterManager;
        
        private List<ShopCard> _cards = new();
        
        [Header("Setups")]
        [SerializeField] private ItemsInProgressSetup _itemsProgressionSetup;

        [Space, SerializeField] private ItemsSetup _itemsSetup;

        
        private IStorage _storage;
        
        private LevelManager _levelManager;
        
        private EventBus _eventBus;

        public void Initialize(IStorage storage, LevelManager levelManager, EventBus eventBus)
        {
            _storage = storage;
            _levelManager = levelManager;
            _eventBus = eventBus;
        }

        private void OnEnable()
        {
            DrawCards();

            if (_filterManager is not null)
            {
                _filterManager.OnFilterChanged += UpdateFilter;
            }
        }

        private void OnDisable()
        {
            ClearCards();
            
            if (_filterManager is not null)
            {
                _filterManager.OnFilterChanged -= UpdateFilter;
            }
        }

        private void DrawCards()
        {
            foreach (var itemProgressionStep in _itemsProgressionSetup.ProgressionSteps)
            {
                var isLocked = itemProgressionStep.Level > _levelManager.CurrentLevel;

                foreach (var itemInStep in itemProgressionStep.AvailableItems)
                {
                    if (_itemsSetup.GetScopeOfItem(itemInStep) != _filterManager.Filter)
                    {
                        continue;
                    }
                    
                    var itemData = _itemsSetup.ItemBindings
                        .SelectMany(binding => binding.Items)
                        .FirstOrDefault(item => item.ItemType == itemInStep);
                    
                    var shopItem = Instantiate(_shopCardPrefab, _cardsRoot);
                    
                    _cards.Add(shopItem);
                
                    shopItem.Initialize(itemData, _storage, _eventBus, isLocked, _filterManager.Filter == ItemScope.Building);
                }
            }
        }

        private void ClearCards()
        {
            foreach (var card in _cards)
            {
                Destroy(card.gameObject);
            }
            
            _cards.Clear();
        }
        
        private void UpdateFilter(ItemScope filter)
        {
            ClearCards();
            DrawCards();
        }
    }
}