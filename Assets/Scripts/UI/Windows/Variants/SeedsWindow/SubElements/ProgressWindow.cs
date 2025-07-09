using System;
using Cultures;
using ProcessBuilding.Systems.GrowingSystems;
using Storage;
using Storage.Items;
using TMPro;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUp.Variants.Garden.Windows.SubElements
{
    public class ProgressWindow : AbstractWindow
    {
        [SerializeField] private Slider _progresSlider;

        [Space, SerializeField] private TMP_Text _timer;

        [Space, SerializeField] private Button _collectButton;
        
        private GrowingSystem _growingSystem;

        public void Initialize(GrowingSystem growingSystem)
        {
            _growingSystem = growingSystem;
        }

        private void Update()
        {
            if (_growingSystem is not null && _growingSystem.Culture is not null)
            {
                _progresSlider.value = _growingSystem.Culture.GrowingProgress;
                _timer.text = $"{(int)(_growingSystem.Culture.GrowingProgress * 100)}%";
            }
        }

        private void OnEnable()
        {
            if (_growingSystem.Culture is not null)
            {
                var isGrown = _growingSystem.Culture.IsGrown;
                
                _collectButton.interactable = isGrown;

                if (!isGrown)
                {
                    _growingSystem.Culture.OnCultureGrown += HandleCultureGrown;
                }
            }

            if (_collectButton is not null)
            {
                _collectButton.onClick.AddListener(TakeResources);
            }
        }

        private void OnDisable()
        {
            if (_collectButton is not null)
            {
                _collectButton.onClick.RemoveListener(TakeResources);
            }
        }

        private void HandleCultureGrown(Culture culture)
        {
            culture.OnCultureGrown -= HandleCultureGrown;
            
            _collectButton.interactable = true;
        }

        private void TakeResources()
        {
            _growingSystem.Collect();
        }
    }
}