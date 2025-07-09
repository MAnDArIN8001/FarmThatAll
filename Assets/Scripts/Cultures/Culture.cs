using System;
using Cultures.Coniguration;
using DG.Tweening;
using Storage.Items;
using UnityEngine;

namespace Cultures
{
    public abstract class Culture : MonoBehaviour
    {
        public virtual event Action<Culture> OnCultureGrown;

        protected float _currentGrowingTime;

        [SerializeField] protected float _scalingTime;

        [field: SerializeField, Space] public ItemType CultureType { get; private set; }

        [field: SerializeField, Header("Setup"), Space] public CultureSetup CultureSetup { get; protected set; }
        
        private Tween _scalingTween;
        
        public bool IsGrown { get; protected set; }

        public float GrowingProgress => _currentGrowingTime / CultureSetup.GrowingTime;

        public virtual void GrowingTick(float tickTime)
        {
            _currentGrowingTime += tickTime;

            if (_currentGrowingTime >= CultureSetup.GrowingTime)
            {
                _currentGrowingTime = CultureSetup.GrowingTime;
                
                OnCultureGrown?.Invoke(this);
            }
        }

        public virtual void Hide()
        {
            _scalingTween = transform.DOScale(Vector3.zero, _scalingTime).OnComplete(() => Destroy(gameObject));
        }
    }
}