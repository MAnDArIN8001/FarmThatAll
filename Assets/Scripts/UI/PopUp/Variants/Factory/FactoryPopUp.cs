using System;
using System.Linq;
using Building.Factory;
using Storage.Items;
using Storage.Setup;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.PopUp
{
    public class FactoryPopUp : AbstractPopUp
    {
        [Inject] private Storage.Storage _playerStorage;
        
        [SerializeField] private Button collectButton;
        
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text itemCount;
        [SerializeField] private TMP_Text itemName;
        
        [SerializeField] private Image itemImage;
        [SerializeField] private Image progressBarImage;

        [SerializeField] private Button _openDestroyWindowButton;
        [SerializeField] private Button _closeDestroyWindowButton;

        [SerializeField] private DestroyWindow _destroyWindow;

        [SerializeField] private GameObject _tab;
        
        [SerializeField] private ItemsSetup _itemsSetup;

        public FactoryBuilding factory;

        private int _localStorageCount;

        private int LocalStorageCount
        {
            set
            {
                if (value == _localStorageCount)
                    return;
                
                _localStorageCount = value;
                
                itemCount.text = $"At the moment {_localStorageCount.ToString()} pieces are ready";
            }
        }

        public void Setup(FactoryBuilding factory)
        {
            var itemData = _itemsSetup.ItemBindings
                .SelectMany(binding => binding.Items)
                .FirstOrDefault(item => item.ItemType == factory.GeneratedItemType);
            
            itemName.text = itemData?.ItemName;
            itemImage.sprite = itemData?.ItemSprite;
            this.factory = factory;
        }
        
        private void OnEnable()
        {
            if (collectButton == null)
            {
                Debug.LogError("no attached collect button to steel factory popup");
                
                return;
            }
            
            collectButton.onClick.AddListener(CollectSteel);

            if (_openDestroyWindowButton is not null)
            {
                _openDestroyWindowButton.onClick.AddListener(OpenDestroyWindow);
            }

            if (_closeDestroyWindowButton is not null)
            {
                _closeDestroyWindowButton.onClick.AddListener(CloseDestroyWindow);
            }

        }

        private void OnDisable()
        {
            if (collectButton == null)
            {
                Debug.LogError("no attached collect button to steel factory popup");
                
                return;
            }
            
            collectButton.onClick.RemoveListener(CollectSteel);
        }

        private void Update()
        {
            progressBarImage.fillAmount = factory.GenerationPercentage;

            LocalStorageCount = factory.LocalStorage;
        }

        private void CollectSteel()
        {
            _playerStorage.IncreaseItem(factory.GeneratedItemType, factory.Get());
        }

        private void OpenDestroyWindow()
        {
            _destroyWindow.Open();
            _destroyWindow.Initialize(factory.gameObject, this);

            Vector3 targetPosition = _openDestroyWindowButton.transform.position;
            StartCoroutine(MoveOverTime(_tab.transform, targetPosition, 0.5f));
        }

        public void CloseDestroyWindow()
        {
            _destroyWindow.Close();

            Vector3 targetPosition = _closeDestroyWindowButton.transform.position;
            StartCoroutine(MoveOverTime(_tab.transform, targetPosition, 0.5f));
        }

        private System.Collections.IEnumerator MoveOverTime(Transform transform, Vector3 target, float time)
        {
            Vector3 start = transform.position;
            float elapsed = 0f;

            while (elapsed < time)
            {
                transform.position = Vector3.Lerp(start, target, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = target;
        }
    }
}