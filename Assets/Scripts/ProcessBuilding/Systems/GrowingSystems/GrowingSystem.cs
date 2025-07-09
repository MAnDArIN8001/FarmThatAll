using System;
using Cultures;
using Storage.Items;
using UnityEngine;

namespace ProcessBuilding.Systems.GrowingSystems
{
    public abstract class GrowingSystem : MonoBehaviour
    {
        public abstract event Action<float> OnCultureCollect;
        public abstract event Action<Culture> OnCultureChanged;
        
        public Culture Culture { get; protected set; }

        public abstract void SetCulture(ItemType culture);

        public abstract int Collect();
    }
}