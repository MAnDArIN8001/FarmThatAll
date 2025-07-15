using Building.Converter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RecipeView
{
    public class IngredientView : MonoBehaviour
    {
        private Image _ingredientImage;
        private TMP_Text _ingredientAmount;

        public void SetIngredient(Sprite ingredientImage, string ingredientAmount)
        {
            _ingredientImage.sprite = ingredientImage;
            
            _ingredientAmount.text = ingredientAmount;
        }
    }
}