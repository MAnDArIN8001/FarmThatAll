using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Storage.Items;
using UnityEngine;
using Zenject;

namespace Building.Converter
{
    public class ConverterBuilding : MonoBehaviour
    {
        [Inject] private Storage.Storage _storage;
        
        [SerializeField] private int progressBarStep;
        
        private int _timer;
        private int Timer
        {
            get => _timer;
            set
            {
                _timer = value;

                if (_currentRecipe == null)
                {
                    CurrentPercentage = 0f;
                    
                    return;
                }
                
                CurrentPercentage = (float) _timer / _currentRecipe.RecipeDurationMilliseconds;
            }
        }
        
        public float CurrentPercentage { get; private set; }
        
        [field: SerializeField] private List<Recipe> availableRecipes { get; set; }
        
        public IReadOnlyList<Recipe> AvailableRecipes => availableRecipes;

        private Queue<Recipe> _recipesInQueue;
        
        public IReadOnlyCollection<Recipe> RecipesInQueue => _recipesInQueue;

        private Recipe _currentRecipe;
        
        public Recipe CurrentRecipe => _currentRecipe;

        private Dictionary<ItemType, int> _tempItemsToEnqueue;
        
        private UniTask _convertTask;
        private CancellationToken _token;
        
        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
            
            _tempItemsToEnqueue = new Dictionary<ItemType, int>();
        }

        private async void Start()
        {
            try
            {
                _convertTask = Convert(_token);

                await _convertTask;
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Converting stopped");
            }
        }

        public void EnqueueRecipe(Recipe recipe)
        {
            if (recipe == null)
            {
                Debug.Log("Unable to enqueue a null recipe");
                
                return;
            }

            _tempItemsToEnqueue.Clear();
            
            foreach (var recipeIngredient in recipe.Ingredients)
            {
                if (_storage.GetItemsCount(recipeIngredient.Type) < recipeIngredient.Amount)
                {
                    Debug.Log("Not enough ingredients to produce a recipe");
                    
                    _tempItemsToEnqueue.Clear();
                    
                    return;
                }
                
                _tempItemsToEnqueue.Add(recipeIngredient.Type, recipeIngredient.Amount);
            }

            foreach (var tempItem in _tempItemsToEnqueue)
            {
                _storage.DecreaseItem(tempItem.Key, tempItem.Value);
            }
            
            _recipesInQueue.Enqueue(recipe);
        }
        
        private async UniTask Convert(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (_recipesInQueue.Count == 0 && _currentRecipe == null)
                {
                    await UniTask.Yield();
                }
                
                await UniTask.Delay(progressBarStep, cancellationToken: token);

                if (_currentRecipe == null)
                {
                    _currentRecipe = _recipesInQueue.Dequeue();

                    Timer = 0;
                }

                Timer += progressBarStep;

                if (Timer >= _currentRecipe.RecipeDurationMilliseconds)
                {
                    _storage.IncreaseItem(_currentRecipe.RecipeOutputType, _currentRecipe.RecipeOutputAmount);
                    
                    Timer -= _currentRecipe.RecipeDurationMilliseconds;
                    _currentRecipe = null;
                }
            }
        }
    }
}