using System;
using System.Collections.Generic;
using Storage.Items;
using UnityEngine;

namespace Building.Converter
{
    [CreateAssetMenu(menuName = "Buildings/NewConverterRecipe")]
    public class Recipe : ScriptableObject
    {
        [field: SerializeField] public string RecipeName { get; private set; }

        [field: SerializeField] public int RecipeDurationMilliseconds { get; private set; } = 5000;
        
        [field: SerializeField] public ItemType RecipeOutputType { get; private set; }
        [field: SerializeField] public int RecipeOutputAmount { get; private set; } = 1;
        
        [field: SerializeField] public Ingredient Ingredient { get; private set; }
        
    }

    [Serializable]
    public class Ingredient
    {
        [field: SerializeField] public ItemType Type { get; private set; }
        
        [field: SerializeField] public int Amount { get; private set; }
    }
}