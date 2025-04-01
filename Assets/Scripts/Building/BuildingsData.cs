using System;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    [CreateAssetMenu(menuName = "Game/BuildingsData")]
    public class BuildingsData : ScriptableObject
    {
        [field:SerializeField] public List<Building> Buildings {get; private set;}
    }

    [Serializable]
    public class Building
    {
        [field:SerializeField] public string Name {get; private set;}
        [field:SerializeField] public string Description {get; private set;}
        [field:SerializeField] public GameObject BuildingPrefab {get; private set;}
        
        [SerializeField] private float interactionWithObjectsOffset = 1f;
        public float InteractionWithObjectsOffset => interactionWithObjectsOffset;
        
    }
}