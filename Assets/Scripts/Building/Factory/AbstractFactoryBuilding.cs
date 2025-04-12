using Storage.Items;
using UnityEngine;

namespace Building.Factory
{
    public class AbstractFactoryBuilding : MonoBehaviour
    {
        [SerializeField] private int localStorageCapacity = 100;
        
        protected float Timer;
        
        [field: SerializeField] public GenerationItemSetup GenerationSetup { get; protected set; }

        [field: SerializeField] public ItemType GeneratedItemType { get; protected set; } = ItemType.Steel;
        
        private int _localStorage;
        
        public int LocalStorage
        {
            get => _localStorage;
            protected set
            {
                if (_localStorage == localStorageCapacity)
                {
                    return;
                }
                
                if (value > localStorageCapacity)
                {
                    _localStorage = localStorageCapacity;
                    
                    return;
                }
                
                _localStorage = value;
            }
        }

        public int CollectLocalStorageItems()
        {
            var collectedItemsCount = LocalStorage;            
            
            LocalStorage = 0;

            return collectedItemsCount;
        }
    }
}