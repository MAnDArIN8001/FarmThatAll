using System;
using Cultures.Coniguration;
using UnityEngine;

namespace Cultures
{
    public abstract class Culture : MonoBehaviour
    {
        public virtual event Action OnCultureGrown;

        protected float _currentGrowingTime;
        
        [field: SerializeField, Header("Setup")] public CultureSetup CultureSetup { get; protected set; }

        public bool IsGrown { get; protected set; }

        public float GrowingProgress => _currentGrowingTime / CultureSetup.GrowingTime;

        public virtual void GrowingTick(float tickTime)
        {
            _currentGrowingTime += tickTime;

            if (_currentGrowingTime >= CultureSetup.GrowingTime)
            {
                _currentGrowingTime = CultureSetup.GrowingTime;
                
                OnCultureGrown?.Invoke();
            }
        }
    }
}