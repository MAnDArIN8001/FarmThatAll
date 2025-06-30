using System.Collections.Generic;
using DG.Tweening;
using UI.DragAndDrop.View;
using UI.ElementCard;
using UI.ElementCard.Slot;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.DragAndDrop
{
    public class DraggableCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private ElementCardController _rootCard;
        
        [Space, SerializeField] private DraggingCard _elementCardPrefab;
        private DraggingCard _elementCardInstance;

        private RectTransform _draggableRect;

        private Canvas _canvas;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _elementCardInstance = Instantiate(_elementCardPrefab, transform);
            _elementCardInstance.Initialize(_rootCard.ItemType, _rootCard.ItemScope, _rootCard.Icon);

            _draggableRect = _elementCardInstance.GetComponent<RectTransform>();
            _draggableRect.position = eventData.position;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_draggableRect is null)
            {
                return;
            }

            _draggableRect.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_elementCardInstance is null)
            {
                return;
            }

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            var isDroped = false;

            foreach (var raycastResult in results)
            {
                if (raycastResult.gameObject.TryGetComponent<DropSlot>(out var dropSlot) && dropSlot.CanAccept(_elementCardInstance))
                {
                    dropSlot.AcceptCard(_elementCardInstance);
                    
                    isDroped = true;
                    
                    break;
                }
            }

            if (isDroped)
            {
                _rootCard.DecreaseCount(1);
                
                _elementCardInstance = null;
            }
            else
            {
                _elementCardInstance.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(_elementCardInstance));
            }
        }
    }
}