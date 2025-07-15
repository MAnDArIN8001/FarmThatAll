using System;
using System.Linq;
using DG.Tweening;
using Storage.Items;
using Storage.Setup;
using UI.DragAndDrop.View;
using UnityEngine;

namespace UI.ElementCard.Slot
{
    public class DropSlot : MonoBehaviour
    {
        public event Action<ItemType> OnAcceptCard;

        [SerializeField] private float _movingTime = 0.2f;
        
        [Space, SerializeField] private ItemScope _targetItemScope;

        [Space, SerializeField] private DraggingCard _dragCardPrefab;

        [Space, SerializeField] private ItemsSetup _itemsSetup;
        
        [SerializeField] private DraggingCard _currentCard;

        private Tween _cardMovementTween;

        public bool IsFree => _currentCard is null;

        public bool CanAccept(DraggingCard cardController) =>
            _currentCard is null || cardController.ItemScope == _targetItemScope;

        public void AcceptCard(DraggingCard cardController)
        {
            _currentCard = cardController;
            _currentCard.transform.SetParent(transform);

            _cardMovementTween = _currentCard.transform.DOMove(transform.position, _movingTime);
            
            OnAcceptCard?.Invoke(cardController.ItemType);
        }

        public void InsertCard(ItemType itemType)
        {
            if (_currentCard is not null)
            {
                return;
            }
            
            _currentCard = Instantiate(_dragCardPrefab, transform.position, Quaternion.identity);
            _currentCard.transform.SetParent(transform);
            
            var itemData = _itemsSetup.ItemBindings
                .SelectMany(binding => binding.Items)
                .FirstOrDefault(item => item.ItemType == itemType);
            
            _currentCard.Initialize(itemData.ItemType, ItemScope.Default, itemData.ItemSprite);
        }

        public void Clear()
        {
            Debug.Log(_currentCard);
            
            _currentCard.Hide();
            _currentCard = null;
        }
    }
}