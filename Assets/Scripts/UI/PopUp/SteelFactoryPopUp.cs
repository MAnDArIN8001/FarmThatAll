using System;
using Building.Factory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.PopUp
{
    public class SteelFactoryPopUp : AbstractPopUp
    {
        [Inject] private Storage.Storage _playerStorage;
        
        [SerializeField] private Button collectButton;
        
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text itemCount;
        
        [SerializeField] private Image itemImage;
        [SerializeField] private Image progressBarImage;

        public FactoryBuilding steelFactory;

        private int _localStorageCount;

        private int LocalStorageCount
        {
            set
            {
                if (value == _localStorageCount)
                    return;
                
                _localStorageCount = value;
                
                itemCount.text = _localStorageCount.ToString();
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
            progressBarImage.fillAmount = steelFactory.GenerationPercentage;

            LocalStorageCount = steelFactory.LocalStorage;
        }

        private void CollectSteel()
        {
            //No realization
        }
    }
}