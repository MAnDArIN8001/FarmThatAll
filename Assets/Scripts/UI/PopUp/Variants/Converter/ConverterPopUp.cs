using System.Collections.Generic;
using System.Linq;
using Building.Converter;
using Storage.Setup;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.PopUp.Variants.Converter
{
    public class ConverterPopUp : AbstractPopUp
    {
        [Inject] private Storage.Storage _storage;

        public ConverterBuilding Converter { get; private set; }
        
        [SerializeField] private Image progressBar;
        
        [SerializeField] private List<RecipeView.RecipeView> availableRecipesViews;
        
        [SerializeField] private GameObject recipeSelectionPanel;
        [SerializeField] private GameObject recipeProducingPanel;
        
        [SerializeField] private ItemsSetup _itemsSetup;

        private bool _isRunning;
        
        private float _currentRecipeProgress;
        
        private float CurrentRecipeProgress
        {
            get => _currentRecipeProgress;
            set
            {
                _currentRecipeProgress = value;
                
                progressBar.fillAmount = _currentRecipeProgress;
            }
        }
        
        public void Setup(ConverterBuilding converter)
        {
            Converter = converter;

            Converter.OnChangeConvertingState += ChangeConverterView;
            foreach (var view in availableRecipesViews)
            {
                view.Setup(this, _itemsSetup);
                view.UpdateInfo();
            }
            // Инициализация состояния окон при открытии
            ChangeConverterView(Converter.CurrentRecipe != null);
        }

        private void ChangeConverterView(bool isRunning)
        {
            _isRunning = isRunning;
            recipeSelectionPanel.SetActive(!_isRunning);
            recipeProducingPanel.SetActive(_isRunning);
        }

        private void OnDisable()
        {
            if (Converter != null)
            {
                Converter.OnChangeConvertingState -= ChangeConverterView;
            }
        }

        void Update()
        {
            if (_isRunning)
            {
                CurrentRecipeProgress = Converter.CurrentPercentage;
            }
        }
    }
}