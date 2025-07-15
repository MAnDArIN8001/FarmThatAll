using System;
using System.Linq;
using Cultures;
using Cultures.Coniguration;
using Storage.Items;
using UnityEngine;
using Utiles.Factory;
using Zenject;

namespace ProcessBuilding.Systems.GrowingSystems.Variants
{
    public class BaseGrowingSystem : GrowingSystem
    {
        public override event Action<float> OnCultureCollect;
        public override event Action<Culture> OnCultureChanged;

        [SerializeField] private CultureMappingSetup _mappingSetup;

        [Space, SerializeField] private Transform _cultureGrowingPoint;

        private MonoAbstractFactory _abstractFactory = MonoAbstractFactory.Instance;

        private Storage.Storage _storage = Storage.Storage.Instance;

        public void Initialize(MonoAbstractFactory factory, Storage.Storage storage)
        {
            _abstractFactory = factory;
            _storage = storage;
        }

        public void Update()
        {
            if (Culture is not null && !Culture.IsGrown)
            {
                Culture.GrowingTick(Time.deltaTime);
            }
        }
        
        public override void SetCulture(ItemType culture)
        {
            if (Culture is not null && !Culture.IsGrown)
            {
                return;
            }
            
            var cultureMap = _mappingSetup.Cultures.FirstOrDefault(item => item.SeedType == culture);

            if (cultureMap is null)
            {
                return;
            }

            Culture = _abstractFactory.Create(cultureMap.Culture, _cultureGrowingPoint, _cultureGrowingPoint.position, Quaternion.identity);    
            
            OnCultureChanged?.Invoke(Culture);
        }

        public override int Collect()
        {
            var collected = Culture.CultureSetup.CultureReward;
            
            OnCultureCollect?.Invoke(collected);
            
            _storage.IncreaseItem(Culture.CultureType, collected);
            
            Culture.Hide();
            Culture = null;

            return collected;
        }
    }
}