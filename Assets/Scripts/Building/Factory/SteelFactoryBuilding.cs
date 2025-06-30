using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Storage.Items;
using UnityEngine;

namespace Building.Factory
{
    public class SteelFactoryBuilding : AbstractFactoryBuilding
    {
        [SerializeField] private int millisecondsTimeStep = 100;
        
        public float GenerationPercentage { get; protected set; }
        
        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        private async void Start()
        {
            await Generate(millisecondsTimeStep, _token);
        }
        
        private void OnDisable()
        {
            Timer = 0f;
            LocalStorage = 0;
            GenerationPercentage = 0f;
        }

        private void Update()
        {
            if (Timer >= GenerationSetup.GenerationTime)
            {
                Timer -= GenerationSetup.GenerationTime;
                LocalStorage += GenerationSetup.ProducingItemsCount;
            }
        }

        private async UniTask Generate(int timeStep, CancellationToken token)
        {
            await UniTask.Delay(timeStep, cancellationToken: token);
            
            Timer += timeStep;
            
            GenerationPercentage = Timer / GenerationSetup.GenerationTime;
        }

        public int GetSteel()
        {
            var localStorage = LocalStorage;
            
            LocalStorage = 0;
            
            return localStorage;
        }
    }
}