using System.Collections.Generic;
using System.Linq;
using Storage;
using Storage.Items;
using Storage.Setup;
using UI.Windows.Variants.TerminalWindows.Elements;
using UnityEngine;
using Zenject;

namespace UI.Windows.Variants.TerminalWindows
{
    public class SellWindow : AbstractWindow
    {
        [Header("UI Elements")] 
        [SerializeField] private Transform _cardsRoot;

        [SerializeField, Space] private SellCard _sellCardPrefab;
        
        private List<SellCard> _sellCards = new ();

        [Header("Configuration")] 
        [SerializeField] private ItemsSetup _itemsSetup;

        [Space, SerializeField] private List<ItemScope> _sellingItemsScope;
        
        private readonly IStorage _storage = Storage.Storage.Instance;

        [Inject] private DiContainer _container;


        private void OnEnable()
        {
            DrawCards();
        }

        private void OnDisable()
        {
            ClearCards();
        }

        private void DrawCards()
        {
            foreach (var itemScope in _sellingItemsScope)
            {
                var itemsOfScope = _storage.GetItemsWithScope(itemScope);

                if (itemsOfScope is not null && itemsOfScope.Count > 0)
                {
                    foreach (var item in itemsOfScope)
                    {
                        var itemData = _itemsSetup.ItemBindings
                            .SelectMany(binding => binding.Items)
                            .FirstOrDefault(itemData => itemData.ItemType == item.Item.ItemType);
                        
                        var card =  Instantiate(_sellCardPrefab, _cardsRoot);
                        _container.Inject(card);


                        card.Initialize(_storage,  itemData);
                    }
                }
            }      
        }

        private void ClearCards()
        {
            foreach (var card in _sellCards)
            {
                Destroy(card.gameObject);
            }
            
            _sellCards.Clear();
        }
    }
}