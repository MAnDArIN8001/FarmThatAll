using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Storage.Items;
using UnityEngine;
using Zenject;

namespace Building.Converter
{
    public class ConverterBuilding : MonoBehaviour
    {
        private Storage.Storage _storage = Storage.Storage.Instance;
        
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

        private Recipe _currentRecipe;
        
        public Recipe CurrentRecipe => _currentRecipe;
        
        private bool _isRunning;
        
        private Ingredient _tempIngredient;
        
        private UniTask _convertTask;
        private CancellationToken _token;
        
        public event Action<bool> OnChangeConvertingState;
        
        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
            
            _storage = Storage.Storage.Instance; 
            //_storage.IncreaseItem(ItemType.Wheat, 100);
        }

        public async UniTask StartProduceRecipe(Recipe recipe)
        {
            if (recipe == null)
            {
                Debug.Log("Unable to enqueue a null recipe");
                
                return;
            }

            if (_isRunning || _convertTask.Status == UniTaskStatus.Pending)
            {
                Debug.Log("Already running");
                
                return;
            }

            _tempIngredient = recipe.Ingredient;

            _storage.DecreaseItem(_tempIngredient.Type, _tempIngredient.Amount);

            try
            {
                _isRunning = true;
                OnChangeConvertingState?.Invoke(true);
                
                _currentRecipe = recipe;
                
                _convertTask = Convert(_token);

                await _convertTask;
            }
            catch (OperationCanceledException)
            {
                _storage.IncreaseItem(_tempIngredient.Type, _tempIngredient.Amount);
                
                Debug.Log("Converting stopped");
            }
        }
        
        private async UniTask Convert(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await UniTask.Delay(progressBarStep, cancellationToken: token);
                    Timer += progressBarStep;

                    CurrentPercentage = (float)Timer / _currentRecipe.RecipeDurationMilliseconds;
                    
                    if (Timer >= _currentRecipe.RecipeDurationMilliseconds)
                    {
                        _storage.IncreaseItem(_currentRecipe.RecipeOutputType, _currentRecipe.RecipeOutputAmount);
                        break;
                    }
                }
            }
            finally
            {
                Timer = 0;
                _currentRecipe = null;
                _isRunning = false;
                
                OnChangeConvertingState?.Invoke(false);
            }
        }
    }
}