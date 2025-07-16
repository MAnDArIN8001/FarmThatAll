using System;
using System.Linq;
using Building.Converter;
using Cysharp.Threading.Tasks;
using Storage.Items;
using Storage.Setup;
using TMPro;
using UI.PopUp.Variants.Converter;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RecipeView
{
    [Serializable]
    public class RecipeView : MonoBehaviour
    {
        private ConverterPopUp _converterPopUp;
        
        private ItemsSetup _itemsSetup;
        private Storage.Storage _storage;
        
        [SerializeField] private Recipe recipe;
        
        [SerializeField] private Button makeRecipeButton;
        
        [SerializeField] private TMP_Text recipeNameText;
        [SerializeField] private TMP_Text recipeDurationText;
        
        [SerializeField] private TMP_Text recipeOutputAmountText;
        [SerializeField] private Image recipeOutputImage;
        
        [SerializeField] private TMP_Text recipeInputAmountText;
        [SerializeField] private Image recipeInputImage;
        
        public Recipe Recipe => recipe;
        
        public void Setup(ConverterPopUp converterPopUp, ItemsSetup itemsSetup, Recipe recipe)
        {
            _converterPopUp = converterPopUp;
            _itemsSetup = itemsSetup;
            _storage = Storage.Storage.Instance;
            this.recipe = recipe;
            if (_storage != null)
            {
                _storage.OnStorageItemChanged += HandleStorageChanged;
            }
            makeRecipeButton.onClick.AddListener(CallConverter);
            UpdateButtonInteractable();
        }

        private void OnDisable()
        {
            makeRecipeButton.onClick.RemoveListener(CallConverter);
            if (_storage != null)
            {
                _storage.OnStorageItemChanged -= HandleStorageChanged;
            }
        }

        private void HandleStorageChanged(Storage.Items.ItemType changedType)
        {
            if (changedType == recipe.Ingredient.Type)
            {
                UpdateButtonInteractable();
            }
        }

        private void UpdateButtonInteractable()
        {
            if (_storage != null)
            {
                int available = _storage.GetItemsCount(recipe.Ingredient.Type);
                makeRecipeButton.interactable = available >= recipe.Ingredient.Amount;
            }
            else
            {
                makeRecipeButton.interactable = false;
            }
        }

        public void UpdateInfo()
        {
            recipeNameText.text = recipe.RecipeName;
            recipeDurationText.text = (recipe.RecipeDurationMilliseconds / 100.0f).ToString() + "s";
            
            recipeOutputAmountText.text = recipe.RecipeOutputAmount.ToString();
            recipeOutputImage.sprite = GetItemOfType(recipe.RecipeOutputType);

            recipeInputAmountText.text = recipe.Ingredient.Amount.ToString();
            recipeInputImage.sprite = GetItemOfType(recipe.Ingredient.Type);
            UpdateButtonInteractable();
        }

        private Sprite GetItemOfType(ItemType itemType) => _itemsSetup.ItemBindings
            .SelectMany(binding => binding.Items)
            .FirstOrDefault(item => item.ItemType == itemType)?.ItemSprite;

        private void CallConverter()
        {
            _converterPopUp.Converter.StartProduceRecipe(recipe).Forget();
        }
    }
}