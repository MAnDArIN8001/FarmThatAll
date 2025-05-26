using System;
using Storage;
using Storage.Items;
using UnityEngine;

namespace UI.ElementCard
{
    public class ElementCardController : MonoBehaviour
    {
        public event Action<ElementCardController> OnCardClosed;
        
        private IStorage _storage;

        private StorageItem _itemData;
        
        [SerializeField] private ElementCardView _cardView;

        public ItemScope ItemScope => _itemData.ItemScope;
        public ItemType ItemType => _itemData.Item.ItemType;

        public Sprite Icon => _itemData.Item.ItemSprite;

        public void Initialize(StorageItem item, IStorage storage)
        {
            _itemData = item;
            _storage = storage;
            
            _cardView.Initialize(_itemData.Item.ItemName, _itemData.Count.ToString(), item.Item.ItemSprite);
            
            _itemData.OnDataChanged += HandleDataChanged;
        }

        private void OnDestroy()
        {
            if (_itemData is not null)
            {
                _itemData.OnDataChanged -= HandleDataChanged;
            }
        }

        public void Show()
        {
            _cardView.Show();
        }

        public void IncreaseCount(int increaseValue = 1)
        {
            _storage.IncreaseItem(ItemType, increaseValue);
        }

        public void DecreaseCount(int decreaseValue = 1)
        {
            _storage.DecreaseItem(ItemType, decreaseValue);
        }

        public void Close()
        {
            OnCardClosed?.Invoke(this);
            
            Destroy(gameObject);
        }

        private void HandleDataChanged()
        {
            if (_itemData.Count == 0)
            {
                _cardView.Hide(Close);

                return;
            }
            
            _cardView.UpdateCounter(_itemData.Count.ToString());
        }
    }
}