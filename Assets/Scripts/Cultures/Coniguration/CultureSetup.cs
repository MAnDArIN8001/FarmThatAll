using UnityEngine;

namespace Cultures.Coniguration
{
    [CreateAssetMenu(fileName = "NewCultureSetup", menuName = "Gameplay/Cultures/CultureSetup", order = 0)]
    public class CultureSetup : ScriptableObject
    {
        [field: SerializeField] public float GrowingTime { get; private set; }

        [field: SerializeField, Space] public int CultureReward { get; private set; }
    }
}