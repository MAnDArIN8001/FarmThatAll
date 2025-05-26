using System;
using System.Collections.Generic;
using Storage;
using Storage.Items;
using UI.ElementCard;
using UnityEngine;
using Zenject;

namespace UI.ToolBar
{
    public class CardsBar : MonoBehaviour
    {
        private bool _mustBeReloaded;
        
        [SerializeField] private ItemScope _itemScope;

        [Space, SerializeField] private Transform _rootForCards;

        [Space, SerializeField] private ElementCardController _cardPrefab;
        
        private List<ElementCardController> _cardsList = new();
        
        [Inject] private IStorage _storage;

        private void OnEnable()
        {
            if (_storage is not null)
            {
                _storage.OnStorageItemChanged += HandleStorageUpdate;
            }
            
            _storage.IncreaseItem(ItemType.WheatSeed, 5);
            
            LoadCards();
        }

        private void OnDestroy()
        {
            if (_storage is not null)
            {
                _storage.OnStorageItemChanged -= HandleStorageUpdate;
            }
        }

        private void LoadCards()
        {
            _cardsList.Clear();
            
            var cardsWithScope = _storage.GetItemsWithScope(_itemScope);
            
            Debug.Log(cardsWithScope.Count);

            foreach (var card in cardsWithScope)
            {
                var newCard = Instantiate(_cardPrefab, _rootForCards);
                
                newCard.Initialize(card, _storage);
                newCard.OnCardClosed += HandleCardClosing;
                
                newCard.Show();
                
                _cardsList.Add(newCard);
            }
        }

        private void HandleCardClosing(ElementCardController elementCardController)
        {
            elementCardController.OnCardClosed -= HandleCardClosing;

            _cardsList.Remove(elementCardController);
        }

        private void HandleStorageUpdate(ItemType itemType)
        {
            
        }
    }
}