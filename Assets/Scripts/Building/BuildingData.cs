using UnityEngine;

namespace Building
{
    [CreateAssetMenu(menuName = "Game/BuildingsData")]
    public class BuildingData : ScriptableObject
    {
        [field:SerializeField] public string Name {get; private set;}
        [field:SerializeField] public string Description {get; private set;}
        [field:SerializeField] public GameObject BuildingPrefab {get; private set;}
        
        [field:SerializeField] public float InteractionWithObjectsOffset { get; private set; } = 1f;
        [field:SerializeField] public LayerMask TerrainLayerMask { get; private set; }
    }
}