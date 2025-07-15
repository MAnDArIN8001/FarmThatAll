using System;
using Building.Factory;
using Storage.Items;
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
        
        private void OnEnable()
        {
            if (collectButton == null)
            {
                Debug.LogError("no attached collect button to steel factory popup");
                
                return;
            }
            
            collectButton.onClick.AddListener(CollectSteel);
            itemName.text = factory.GeneratedItemType.ToString();
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
    }
}