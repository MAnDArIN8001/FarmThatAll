using System;
using System.Collections.Generic;
using Storage.Items;
using UnityEngine;

namespace Cultures.Coniguration
{
    [CreateAssetMenu(menuName = "Gameplay/Cultures/CultureMappingSetup", fileName = "NewCultureMappingSetup")]
    public class CultureMappingSetup : ScriptableObject
    {
        [SerializeField] private List<TypedCulture> _cultures;

        public IReadOnlyList<TypedCulture> Cultures => _cultures;
    }
    
    [Serializable]
    public class TypedCulture
    {
        [field: SerializeField] public ItemType SeedType { get; private set; }

        [field: SerializeField, Space] public Culture Culture { get; private set; }
    }
}