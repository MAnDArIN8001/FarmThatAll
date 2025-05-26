using System;
using Cultures;
using UnityEngine;

namespace ProcessBuilding.Systems.GrowingSystems
{
    public abstract class GrowingSystem : MonoBehaviour
    {
        public abstract event Action<float> OnCultureCollect;
            
        public abstract event Action<Culture> OnCultureChanged;

        public Culture Culture { get; protected set; }

        public abstract void SetCulture(Culture culture);

        public abstract float Collect();
    }
}