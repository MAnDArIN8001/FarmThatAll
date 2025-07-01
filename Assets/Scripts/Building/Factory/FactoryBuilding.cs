using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Storage.Items;
using UnityEngine;

namespace Building.Factory
{
    public class FactoryBuilding : AbstractFactoryBuilding
    {
        [SerializeField] private int millisecondsTimeStep = 100;
        
        public float GenerationPercentage { get; protected set; }

        private UniTask _generateTask;
        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        private async void Start()
        {
            try
            {
                _generateTask = Generate(millisecondsTimeStep, _token);
            
                await _generateTask;
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Generation cancelled");
            }
            
        }
        
        private void OnDisable()
        {
            Timer = 0f;
            LocalStorage = 0;
            GenerationPercentage = 0f;

            if (_generateTask.Status == UniTaskStatus.Pending)
            {
                _token.ThrowIfCancellationRequested();
            }
        }

        private async UniTask Generate(int timeStep, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(timeStep, cancellationToken: token);
        
                Timer += timeStep;
                GenerationPercentage = Timer / GenerationSetup.GenerationTime;
        
                if (Timer >= GenerationSetup.GenerationTime)
                {
                    Timer -= GenerationSetup.GenerationTime;
                    LocalStorage += GenerationSetup.ProducingItemsCount;
                }
            }
        }

        public int Get()
        {
            var localStorage = LocalStorage;
            
            LocalStorage = 0;
            
            return localStorage;
        }
    }
}