using System.Collections.Generic;
using Building.Converter;
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
                view.UpdateInfo();
            }
        }

        private void ChangeConverterView(bool isRunning)
        {
            _isRunning = isRunning;
            
            if (_isRunning)
            {
                recipeSelectionPanel.SetActive(false);
                recipeProducingPanel.SetActive(true);
            }
            
            recipeSelectionPanel.SetActive(true);
            recipeProducingPanel.SetActive(false);
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