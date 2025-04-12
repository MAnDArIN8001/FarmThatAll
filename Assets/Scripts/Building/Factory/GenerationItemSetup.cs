using UnityEngine;

namespace Building.Factory
{
    [CreateAssetMenu(menuName = "Buildings/NewGenerationSetup")]
    public class GenerationItemSetup : ScriptableObject
    {
        [field: SerializeField] public float GenerationTime { get; private set; }
        
        [field: SerializeField] public int ProducingItemsCount { get; private set; }
    }
}