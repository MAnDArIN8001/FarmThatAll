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
        
        [SerializeField] private Recipe recipe;
        
        [SerializeField] private Button makeRecipeButton;
        
        [SerializeField] private TMP_Text recipeNameText;
        [SerializeField] private TMP_Text recipeDurationText;
        
        [SerializeField] private TMP_Text recipeOutputAmountText;
        [SerializeField] private Image recipeOutputImage;
        
        [SerializeField] private TMP_Text recipeInputAmountText;
        [SerializeField] private Image recipeInputImage;
        
        public void Setup(ConverterPopUp converterPopUp, ItemsSetup itemsSetup)
        {
            _converterPopUp = converterPopUp;
            
            _itemsSetup = itemsSetup;
            
            makeRecipeButton.onClick.AddListener(CallConverter);
        }

        private void OnDisable()
        {
            makeRecipeButton.onClick.RemoveListener(CallConverter);
        }

        public void UpdateInfo()
        {
            recipeNameText.text = recipe.RecipeName;
            recipeDurationText.text = (recipe.RecipeDurationMilliseconds / 1000.0f).ToString();
            
            recipeOutputAmountText.text = recipe.RecipeOutputAmount.ToString();
            recipeOutputImage.sprite = GetItemOfType(recipe.RecipeOutputType);

            recipeInputAmountText.text = recipe.Ingredient.Amount.ToString();
            recipeInputImage.sprite = GetItemOfType(recipe.Ingredient.Type);
        }

        private Sprite GetItemOfType(ItemType itemType)
        {
            var itemScope = _itemsSetup.GetScopeOfItem(itemType);
            
            foreach (var binding in _itemsSetup.ItemBindings)
            {
                if (binding.ItemScope != itemScope)
                    continue;

                foreach (var item in binding.Items)
                {
                    if (item.ItemType == recipe.RecipeOutputType)
                    {
                        return item.ItemSprite;
                    }
                }
            }
            
            return null;
        }

        private void CallConverter()
        {
            _converterPopUp.Converter.StartProduceRecipe(recipe).Forget();
        }
    }
}