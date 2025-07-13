using System;
using Storage.Items;
using UnityEngine;

namespace UI.Filter
{
    public class ScopeFilterManager : MonoBehaviour
    {
        public event Action<ItemScope> OnFilterChanged;

        [field: SerializeField] public ItemScope Filter { get; private set; }

        public void SetFilter(ItemScope filter)
        {
            Filter = filter;
            
            OnFilterChanged?.Invoke(Filter);
        }
    }
}