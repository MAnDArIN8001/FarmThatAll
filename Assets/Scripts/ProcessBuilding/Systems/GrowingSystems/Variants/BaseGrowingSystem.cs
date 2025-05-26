using System;
using Cultures;
using UnityEngine;
using Utiles.Factory;
using Zenject;

namespace ProcessBuilding.Systems.GrowingSystems.Variants
{
    public class BaseGrowingSystem : GrowingSystem
    {
        public override event Action<float> OnCultureCollect;
        public override event Action<Culture> OnCultureChanged;

        [SerializeField] private Transform _cultureGrowingPoint;

        [Inject] private MonoAbstractFactory _abstractFactory;

        public void Update()
        {
            if (Culture is not null && !Culture.IsGrown)
            {
                Culture.GrowingTick(Time.deltaTime);
            }
        }
        
        public override void SetCulture(Culture culturePrefab)
        {
            if (Culture is not null && !Culture.IsGrown)
            {
                return;
            }

            Culture = _abstractFactory.Create(culturePrefab, _cultureGrowingPoint, _cultureGrowingPoint.position, Quaternion.identity);

            Culture.OnCultureGrown += HandleCultureGrown;
            
            OnCultureChanged?.Invoke(Culture);
        }

        public override float Collect()
        {
            OnCultureCollect?.Invoke(Culture.CultureSetup.CultureReward);

            return Culture.CultureSetup.CultureReward;
        }

        private void HandleCultureGrown()
        {
            Culture.OnCultureGrown -= HandleCultureGrown;
        }
    }
}