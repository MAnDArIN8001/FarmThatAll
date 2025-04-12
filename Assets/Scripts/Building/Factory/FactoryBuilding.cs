using System;
using System.Collections;
using Storage.Items;
using UnityEngine;

namespace Building.Factory
{
    public class FactoryBuilding : AbstractFactoryBuilding
    {
        public float GenerationPercentage { get; protected set; }

        private void OnDisable()
        {
            Timer = 0f;
            LocalStorage = 0;
            GenerationPercentage = 0f;
        }

        private void Update()
        {
            Timer += Time.deltaTime;
            
            GenerationPercentage = Timer / GenerationSetup.GenerationTime;
            
            if (Timer >= GenerationSetup.GenerationTime)
            {
                Timer -= GenerationSetup.GenerationTime;
                LocalStorage += GenerationSetup.ProducingItemsCount;
            }
        }
    }
}