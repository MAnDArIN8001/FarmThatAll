using System;
using System.Collections.Generic;
using Storage.Items;
using UnityEngine;

namespace Ship
{
    [CreateAssetMenu(fileName = "ItemsProgressionSetup", menuName = "Gameplay/Progression", order = 0)]
    public class ItemsInProgressSetup : ScriptableObject
    {
        [SerializeField] private List<ProgressionStep> _progressionSteps;
        
        public IReadOnlyList<ProgressionStep> ProgressionSteps => _progressionSteps;
    }

    [Serializable]
    public class ProgressionStep
    {
        [field: SerializeField] public int Level { get; private set; }

        [SerializeField] private List<ItemType> _availableItems;
        
        public IReadOnlyList<ItemType> AvailableItems => _availableItems;
    }
}