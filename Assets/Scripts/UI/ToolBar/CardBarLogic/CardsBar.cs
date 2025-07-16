using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private IStorage _storage;

        public void Initialize(IStorage storage)
        {
            _storage = storage;
        }

        private void OnEnable()
        {
            LoadCards();
        }

        private void LoadCards()
        {
            ClearCardsList();
            
            var cardsWithScope = _storage.GetItemsWithScope(_itemScope);

            foreach (var card in cardsWithScope)
            {
                var newCard = Instantiate(_cardPrefab, _rootForCards);
                
                newCard.Initialize(card, _storage);
                newCard.OnCardClosed += HandleCardClosing;
                
                newCard.Show();
                
                _cardsList.Add(newCard);
            }
        }

        private void ClearCardsList()
        {
            foreach (var card in _cardsList)
            {
                Destroy(card.gameObject);
            }
            
            _cardsList.Clear();
        }
        
        private void HandleCardClosing(ElementCardController elementCardController)
        {
            elementCardController.OnCardClosed -= HandleCardClosing;

            _cardsList.Remove(elementCardController);
        }
    }
}