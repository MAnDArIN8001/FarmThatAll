using System;
using DG.Tweening;
using Storage.Items;
using UI.DragAndDrop.View;
using UnityEngine;

namespace UI.ElementCard.Slot
{
    public class DropSlot : MonoBehaviour
    {
        public event Action<ItemType> OnAcceptCard;

        [SerializeField] private float _movingTime = 0.2f;
        
        [Space, SerializeField] private ItemScope _targetItemScope;
        
        private DraggingCard _currentCard;

        private Tween _cardMovementTween;

        public bool CanAccept(DraggingCard cardController) =>
            _currentCard is null || cardController.ItemScope == _targetItemScope;

        public void AcceptCard(DraggingCard cardController)
        {
            _currentCard = cardController;
            _currentCard.transform.SetParent(transform);

            _cardMovementTween = _currentCard.transform.DOMove(transform.position, _movingTime);
            
            OnAcceptCard?.Invoke(cardController.ItemType);
        }
    }
}